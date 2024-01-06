using Microsoft.EntityFrameworkCore;
using Recept.Entity.Generated;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recept.Data;

namespace Recept.Repositories
{
    public interface IAllergenRepository
    {
        Task<List<Allergen>> GetAllAsync();
        Task<Allergen> GetByIdAsync(int id);
        Task CreateAsync(Allergen allergen);
        Task UpdateAsync(Allergen allergen);
        Task DeleteAsync(int id);
        Task<List<Allergen>> GetByAllergenIdsAsync(List<int> allergenIds);
    }

    public class AllergenRepository : IAllergenRepository
    {
        private readonly ReceptekContext _dbContext;

        public AllergenRepository(ReceptekContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Allergen>> GetAllAsync()
        {
            return await _dbContext.Allergenek.ToListAsync();
        }

        public async Task<Allergen> GetByIdAsync(int id)
        {
            return await _dbContext.Allergenek.FirstAsync(r => r.Id == id);
        }

        public async Task CreateAsync(Allergen allergen)
        {
            _dbContext.Allergenek.Add(allergen);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Allergen allergen)
        {
            _dbContext.Entry(allergen).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var allergen = await _dbContext.Allergenek.FindAsync(id);
            if (allergen != null)
            {
                _dbContext.Remove(allergen);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Allergen>> GetByAllergenIdsAsync(List<int> allergenIds)
        {
            // Az allergének lekérése azonosítók alapján
            return await _dbContext.Allergenek
                .Where(allergen => allergenIds.Contains(allergen.Id))
                .ToListAsync();
        }
    }
}
