using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;

namespace Recept.Repositories
{
    public interface IKategoriaRepository
    {
        Task<List<Kategorium>> GetAllAsync();
        Task<Kategorium> GetByIdAsync(int id);
        Task CreateAsync(Kategorium kategoria);
        Task UpdateAsync(Kategorium kategoria);
        Task DeleteAsync(int id);
        Task<bool> VanFuggosegAsync(int id);
    }

    public class KategoriaRepository : IKategoriaRepository
    {
        private readonly ReceptekContext _dbContext;

        public KategoriaRepository(ReceptekContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Kategorium> GetByIdAsync(int id)
        {
            return await _dbContext.Kategoria.FirstAsync(r => r.Id == id && !r.Deleted);
        }

        public async Task<List<Kategorium>> GetAllAsync()
        {
            return await _dbContext.Kategoria.ToListAsync();
        }

        public async Task CreateAsync(Kategorium kategoria)
        {
            _dbContext.Add(kategoria);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Kategorium kategoria)
        {
            _dbContext.Entry(kategoria).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var kategoria = await _dbContext.Kategoria.FindAsync(id);
            if (kategoria != null)
            {
                _dbContext.Remove(kategoria);
                await _dbContext.SaveChangesAsync();
            }
     
        }
        public async Task<bool> VanFuggosegAsync(int id)
        {
            return await _dbContext.Alapanyagok.AnyAsync(rh => rh.KategoriaId == id && !rh.Kategoria.Deleted);
        }
    }
}
