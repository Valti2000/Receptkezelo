using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Repositories
{
    public interface ICsoportRepository
    {
        Task<List<Csoport>> GetAllAsync();
        Task<Csoport> GetByIdAsync(int id);
        Task AddAsync(Csoport csoport);
        Task UpdateAsync(Csoport csoport);
        Task DeleteAsync(int id);
    }

    public class CsoportRepository : ICsoportRepository
    {
        private readonly ReceptekContext _dbContext;

        public CsoportRepository(ReceptekContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Csoport>> GetAllAsync()
        {
            return await _dbContext.Csoport.ToListAsync();
        }

        public async Task<Csoport> GetByIdAsync(int id)
        {
            return await _dbContext.Csoport.FirstAsync(h => h.Id == id);
        }

        public async Task AddAsync(Csoport csoport)
        {
            _dbContext.Add(csoport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Csoport csoport)
        {
            _dbContext.Entry(csoport).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var csoport = await _dbContext.Csoport.FindAsync(id);
            if (csoport != null)
            {
                _dbContext.Remove(csoport);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
