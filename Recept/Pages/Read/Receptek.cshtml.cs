using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recept.Pages.Read
{
    public class ReceptekModel : PageModel
    {
        private readonly IReceptRepository _receptRepository;
        private readonly ReceptekContext _dbContext;

        [BindProperty]
        public int ElokeszitesiIdo { get; set; }

        public ReceptekModel(IReceptRepository receptRepository, ReceptekContext dbContext)
        {
            _receptRepository = receptRepository;
            _dbContext = dbContext;
        }

        public IEnumerable<Receptek> Receptek { get; set; } = new List<Receptek>();

        public async Task OnGetAsync()
        {
            Receptek = await _receptRepository.GetAllAsync();
        }

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }


        public async Task OnPostAsync()
        {
            if (IsDeleted)
            {
                Receptek = await _dbContext.Receptek.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                Receptek = await _receptRepository.GetAllAsync();
            }
        }


        public async Task<List<Hozzavalo>> GetHozzavalokByReceptIdAsync(int receptId)
        {
            return await _receptRepository.GetHozzavalokByReceptIdAsync(receptId);
        }

        public async Task<List<Allergen>> GetAllergenekByReceptIdAsync(int receptId)
        {
            var result = await _receptRepository.GetAllergenekByReceptIdAsync(receptId);

            return result;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostToggleKedvenc(int receptId)
        {

            // Kedvenc állapot frissítése az adatbázisban
            var felhasznaloId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _receptRepository.ToggleKedvencAsync(felhasznaloId, receptId);

            // Visszatérés a receptek oldalra vagy az elõzõ oldalra
            return RedirectToPage();
        }
    }
}
