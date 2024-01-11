using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Read
{
    [Authorize(Roles = "Admin , ReceptIro")]
    public class ReadCsoportModel : PageModel
    {
        private readonly ICsoportRepository _csoportRepository;
        private readonly IHozzavaloRepository _hozzavaloRepository;
        private readonly ReceptekContext _dbContext;

        public ReadCsoportModel(ICsoportRepository csoportRepository, IHozzavaloRepository hozzavaloRepository, ReceptekContext dbContext)
        {
            _csoportRepository = csoportRepository;
            _hozzavaloRepository = hozzavaloRepository;
            _dbContext = dbContext;
        }

        public List<Csoport> Csoportok { get; set; } = new List<Csoport>();

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }

        public async Task OnGetAsync()
        {
            Csoportok = await _csoportRepository.GetAllAsync();
        }

        public async Task OnPostAsync()
        {
            if (IsDeleted)
            {
                Csoportok = await _dbContext.Csoport.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                Csoportok = await _csoportRepository.GetAllAsync();
            }
        }

        public async Task<List<Hozzavalo>> GetHozzavaloByCsoportIdAsync(int csoportId)
        {
            return await _hozzavaloRepository.GetByCsoportIdAsync(csoportId);
        }

    }
}
