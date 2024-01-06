// HozzavaloRepository.cs
using Microsoft.EntityFrameworkCore;
using Recept.Entity.Generated;
using Recept.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Repositories
{
    public interface IHozzavaloRepository
    {
        Task<List<Hozzavalo>> GetAllAsync();
        Task<Hozzavalo> GetByIdAsync(int id);
        Task CreateAsync(Hozzavalo hozzavalo);
        Task UpdateAsync(Hozzavalo hozzavalo);
        Task DeleteAsync(int id);
        Task<int> ReturnIdAsync(Hozzavalo hozzavalo);
        Task<List<Hozzavalo>> GetByCsoportIdAsync(int csoportId);
    }

    public class HozzavaloRepository : IHozzavaloRepository
    {
        private readonly ReceptekContext _dbContext;

        public HozzavaloRepository(ReceptekContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Hozzavalo>> GetAllAsync()
        {
            return await _dbContext.Hozzavalok.ToListAsync();
        }

        public async Task<Hozzavalo> GetByIdAsync(int id)
        {
            return await _dbContext.Hozzavalok.FirstAsync(h => h.Id == id);
        }

        public async Task CreateAsync(Hozzavalo hozzavalo)
        {
            _dbContext.Hozzavalok.Add(hozzavalo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hozzavalo hozzavalo)
        {
            _dbContext.Entry(hozzavalo).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hozzavalo = await _dbContext.Hozzavalok.FindAsync(id);
            if (hozzavalo != null)
            {
                _dbContext.Hozzavalok.Remove(hozzavalo);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> ReturnIdAsync(Hozzavalo hozzavalo)
        {
            await Task.Delay(100); 

            return hozzavalo.Id;
        }
        public async Task<List<Hozzavalo>> GetByCsoportIdAsync(int csoportId)
        {
            return await _dbContext.Hozzavalok.Where(h => h.CsoportId == csoportId).ToListAsync();
        }
    }
}
