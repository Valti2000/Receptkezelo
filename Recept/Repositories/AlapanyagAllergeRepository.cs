using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recept.Repositories
{
    public interface IAlapanyagAllergenRepository
    {
        Task<List<AlapanyagAllergen>> GetAllAsync();
        Task<List<AlapanyagAllergen>> GetByAlapanyagIdAsync(int id);
        Task CreateAsync(AlapanyagAllergen alapanyagAllergen);
        Task UpdateAsync(AlapanyagAllergen alapanyagAllergen);
        Task DeleteAsync(int id);
        Task DeleteAllByAlapanyagIdAsync(int alapanyagId);
    }

    public class AlapanyagAllergenRepository : IAlapanyagAllergenRepository
    {
        private readonly ReceptekContext _dbContext;

        public AlapanyagAllergenRepository(ReceptekContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AlapanyagAllergen>> GetAllAsync()
        {
            return await _dbContext.AlapanyagAllergen.ToListAsync();
        }

        public async Task<List<AlapanyagAllergen>> GetByAlapanyagIdAsync(int alapanyagId)
        {
            return await _dbContext.AlapanyagAllergen
                .Where(rh => rh.AlapanyagId == alapanyagId)
                .ToListAsync();
        }

        public async Task CreateAsync(AlapanyagAllergen alapanyagAllergen)
        {
            _dbContext.AlapanyagAllergen.Add(alapanyagAllergen);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(AlapanyagAllergen alapanyagAllergen)
        {
            _dbContext.Entry(alapanyagAllergen).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alapanyagAllergen = await _dbContext.AlapanyagAllergen.FindAsync(id);
            if (alapanyagAllergen != null)
            {
                _dbContext.Remove(alapanyagAllergen);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAllByAlapanyagIdAsync(int alapanyagId)
        {
            var allergenRecords = await _dbContext.AlapanyagAllergen
                .Where(aa => aa.AlapanyagId == alapanyagId)
                .ToListAsync();

            if (allergenRecords.Any())
            {
                _dbContext.AlapanyagAllergen.RemoveRange(allergenRecords);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
