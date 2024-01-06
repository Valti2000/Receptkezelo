using Microsoft.EntityFrameworkCore;
using Recept.Entity.Generated;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recept.Data;
using System.ComponentModel.DataAnnotations;


namespace Recept.Repositories
{
    public interface IAlapanyagRepository
    {
        Task<List<Alapanyag>> GetAllAsync();
        Task<Alapanyag> GetByIdAsync(int id);
        Task CreateAsync(Alapanyag alapanyag);
        Task UpdateAsync(Alapanyag alapanyag);
        Task DeleteAsync(int id);
        Task<int?> GetKategoriaIdAsync(int alapanyagId);
        Task<List<int>> GetAlapanyagIdByKategoriaIdAsync(int kategoriaId);
        Task<List<Alapanyag>> GetAlapanyagokByIdsAsync(List<int> alapanyagIds);
        Task<List<Alapanyag>> GetAlapanyagokByAllergenIdAsync(int allergenId);
        Task<List<Allergen>> GetAllergenByAlapanyagIdAsync(int alapanyagId);
    }

    public class AlapanyagRepository : IAlapanyagRepository
    {
        private readonly ReceptekContext _dbContext;

        private readonly IAlapanyagAllergenRepository _alapanyagAllergenRepository;

        private readonly IAllergenRepository _allergenRepository;

        public AlapanyagRepository(ReceptekContext dbContext, IAlapanyagAllergenRepository alapanyagAllergenRepository, IAllergenRepository allergenRepository)
        {
            _dbContext = dbContext;
            _alapanyagAllergenRepository = alapanyagAllergenRepository;
            _allergenRepository = allergenRepository;
        }

        public async Task<List<Alapanyag>> GetAllAsync()
        {
            return await _dbContext.Alapanyagok.ToListAsync();
        }

        public async Task<Alapanyag> GetByIdAsync(int id)
        {
            return await _dbContext.Alapanyagok.FirstAsync(a => a.Id == id);
        }

        public async Task CreateAsync(Alapanyag alapanyag)
        {
            _dbContext.Alapanyagok.Add(alapanyag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Alapanyag alapanyag)
        {
            _dbContext.Entry(alapanyag).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alapanyag = await _dbContext.Alapanyagok.FindAsync(id);
            if (alapanyag != null)
            {
                _dbContext.Alapanyagok.Remove(alapanyag);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int?> GetKategoriaIdAsync(int alapanyagId)
        {
            var alapanyag = await _dbContext.Alapanyagok.FindAsync(alapanyagId);

            return alapanyag?.KategoriaId;
        }

        public async Task<List<int>> GetAlapanyagIdByKategoriaIdAsync(int kategoriaId)
        {
            if (kategoriaId <= 0)
            {
                throw new ArgumentException("Invalid kategoriaId");
            }

            var alapanyagIdsByKategoria = await _dbContext.Alapanyagok
                .Where(a => a.KategoriaId == kategoriaId)
                .Select(a => a.Id)
                .ToListAsync();

            return alapanyagIdsByKategoria;
        }

        public async Task<List<Alapanyag>> GetAlapanyagokByIdsAsync(List<int> alapanyagIds)
        {
            var alapanyagok = await _dbContext.Alapanyagok
                .Where(a => alapanyagIds.Contains(a.Id))
                .ToListAsync();

            return alapanyagok;
        }
        public async Task<List<Alapanyag>> GetAlapanyagokByAllergenIdAsync(int allergenId)
        {
            var alapanyagok = await _dbContext.Alapanyagok
                .Where(a => a.AlapanyagAllergens.Any(ka => ka.AllergenId == allergenId))
                .ToListAsync();

            return alapanyagok;
        }

        public async Task<List<Allergen>> GetAllergenByAlapanyagIdAsync(int alapanyagId)
        {
            if (alapanyagId <= 0)
            {
                throw new ArgumentException("Invalid alapanyagId");
            }

            var alapanyagAllergenList = await _alapanyagAllergenRepository.GetByAlapanyagIdAsync(alapanyagId);

            var allergenIds = alapanyagAllergenList.Select(rh => rh.AllergenId).ToList();

            // Ha nincs allergén, üres listát adj vissza
            if (allergenIds.Count == 0)
            {
                return new List<Allergen>();
            }

            // Most az allergenIds-ből leképezés segítségével kérdezzük le az allergéneket az AllergenRepository-ból
            var allergens = await _allergenRepository.GetByAllergenIdsAsync(allergenIds);

            return allergens;
        }
    }
}