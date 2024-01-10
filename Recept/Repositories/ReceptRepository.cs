using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;

namespace Recept.Repositories
{
    public interface IReceptRepository
    {
        Task<List<Receptek>> GetAllAsync();
        Task<Receptek> GetByIdAsync(int id);
        Task AddAsync(Receptek recept);
        Task UpdateAsync(Receptek recept);
        Task DeleteAsync(int id);
        Task<List<Hozzavalo>> GetHozzavalokByReceptIdAsync(int receptId);

        Task<List<Hozzavalo>> GetHozzavalokAsync();

        Task<List<Allergen>> GetAllergenekByReceptIdAsync(int AlapanyagId);

    }

    public class ReceptRepository : IReceptRepository
    {
        private readonly ReceptekContext _dbContext;

        private readonly IReceptHozzavaloRepository _receptHozzavaloRepository;

        private readonly IAllergenRepository _allergenRepository;

        public ReceptRepository(ReceptekContext dbContext, IReceptHozzavaloRepository receptHozzavaloRepository, IAllergenRepository allergenRepository)
        {
            _dbContext = dbContext;
            _receptHozzavaloRepository = receptHozzavaloRepository;
            _allergenRepository = allergenRepository;
        }


        public async Task<List<Receptek>> GetAllAsync()
        {
            return await _dbContext.Receptek
                .Include(r => r.ReceptHozzavalok)
                    .ThenInclude(rh => rh.Hozzavalo)
                .ToListAsync();
        }

        public async Task<Receptek> GetByIdAsync(int id)
        {
            return await _dbContext.Receptek.FirstAsync(r => r.Id == id);
        }

        public async Task<List<Hozzavalo>> GetHozzavalokAsync()
        {
            return await _dbContext.Hozzavalok.ToListAsync();
        }

        public async Task AddAsync(Receptek recept)
        {
            _dbContext.Add(recept);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Receptek recept)
        {
            _dbContext.Entry(recept).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var receptHozzavaloToDelete = _dbContext.ReceptHozzavalo.Where(rh => rh.ReceptId == id);

            _dbContext.ReceptHozzavalo.RemoveRange(receptHozzavaloToDelete);
            var recept = await _dbContext.Receptek.FindAsync(id);

            if (recept == null)
            {
                throw new InvalidOperationException($"Recept with Id {id} not found.");
            }

            _dbContext.Receptek.Remove(recept);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Hozzavalo>> GetHozzavalokByReceptIdAsync(int receptId)
        {
            if (receptId <= 0)
            {
                throw new ArgumentException("Invalid receptId");
            }

            var receptHozzavaloList = await _receptHozzavaloRepository.GetByReceptIdAsync(receptId);

            var hozzavaloList = receptHozzavaloList.Select(rh => rh.Hozzavalo).ToList();

            return hozzavaloList;
        }

        public async Task<List<Allergen>> GetAllergenekByReceptIdAsync(int AlapanyagId)
        {

            var allergenIds = await _dbContext.AlapanyagAllergen
                                                .Where(ha => ha.AlapanyagId == AlapanyagId)
                                                .Select(ha => ha.AllergenId)
                                                .ToListAsync();

            return await _allergenRepository.GetByAllergenIdsAsync(allergenIds);
        }

        public async Task ToggleKedvencAsync(string felhasznaloId, int receptId)
        {

            var recept = await _dbContext.Receptek.FindAsync(receptId);
            if (recept == null)
            {

                throw new ArgumentException("A megadott recept nem létezik.");
            }

            var kedvencRecord = await _dbContext.KedvencRecept.FindAsync(felhasznaloId, receptId);

            if (kedvencRecord != null)
            {
                _dbContext.KedvencRecept.Remove(kedvencRecord);
            }
            else
            {
                _dbContext.KedvencRecept.Add(new KedvencRecept
                {
                    UserId = felhasznaloId,
                    ReceptId = receptId
                });
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
