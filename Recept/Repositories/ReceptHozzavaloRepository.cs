using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Repositories
{
    public interface IReceptHozzavaloRepository
    {
        Task<IEnumerable<ReceptHozzavalo>> GetAllAsync();
        Task<List<ReceptHozzavalo>> GetByReceptIdAsync(int receptId);
        Task AddAsync(ReceptHozzavalo receptHozzavalo);
        Task DeleteAsync(int id);
    }
    public class ReceptHozzavaloRepository : IReceptHozzavaloRepository
    {
        private readonly ReceptekContext _context;

        public ReceptHozzavaloRepository(ReceptekContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceptHozzavalo>> GetAllAsync()
        {
            return await _context.ReceptHozzavalo.ToListAsync();
        }

        public async Task<List<ReceptHozzavalo>> GetByReceptIdAsync(int receptId)
        {
            return await _context.ReceptHozzavalo
                .Where(rh => rh.ReceptId == receptId)
                .ToListAsync();
        }


        public async Task AddAsync(ReceptHozzavalo receptHozzavalo)
        {
            _context.ReceptHozzavalo.Add(receptHozzavalo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var receptHozzavalo = await _context.ReceptHozzavalo.FindAsync(id);
            if (receptHozzavalo != null)
            {
                _context.ReceptHozzavalo.Remove(receptHozzavalo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
