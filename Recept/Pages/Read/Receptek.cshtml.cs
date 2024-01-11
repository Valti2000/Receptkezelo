using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin , ReceptIro, ReceptOlvaso")]
    public class ReceptekModel : PageModel
    {
        private readonly IReceptRepository _receptRepository;
        private readonly ReceptekContext _dbContext;

        [BindProperty]
        public int ElokeszitesiIdo { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool CsakKedvencek { get; set; }

        public string ReceivedToken { get; set; } = string.Empty;

        public ReceptekModel(IReceptRepository receptRepository, ReceptekContext dbContext)
        {
            _receptRepository = receptRepository;
            _dbContext = dbContext;
        }

        public IEnumerable<Receptek> Receptek { get; set; } = new List<Receptek>();

        public async Task OnGetAsync()
        {
            var felhasznaloId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var jwtToken = HttpContext.User?.FindFirstValue("sub");

            // Beállítjuk a ReceivedToken értékét
            ReceivedToken = jwtToken ?? string.Empty;

            Console.WriteLine("ReceivedToken: " + ReceivedToken);

            if (CsakKedvencek)
            {
                Receptek = await _receptRepository.GetKedvencReceptByUserIdAsync(felhasznaloId);
            }
            else
            {
                Receptek = await _receptRepository.GetAllAsync();
            }
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

        public async Task<IActionResult> OnPostToggleKedvenc(int receptId)
        {
            var felhasznaloId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var kedvencRecept = await _dbContext.KedvencRecept
                .FirstOrDefaultAsync(k => k.ReceptId == receptId && k.UserId == felhasznaloId);

            if (kedvencRecept == null)
            {
                kedvencRecept = new KedvencRecept { ReceptId = receptId, UserId = felhasznaloId };
                _dbContext.KedvencRecept.Add(kedvencRecept);
            }
            else
            {
                _dbContext.KedvencRecept.Remove(kedvencRecept);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToPage(new { page = "/Read/Receptek" });
        }


        public async Task<IActionResult> OnPostRemoveFromFavorites(int receptId)
        {
            var felhasznaloId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var kedvencRecept = await _dbContext.KedvencRecept
                .FirstOrDefaultAsync(k => k.ReceptId == receptId && k.UserId == felhasznaloId);

            if (kedvencRecept != null)
            {
                _dbContext.KedvencRecept.Remove(kedvencRecept);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage(new { page = "/Read/Receptek" });
        }
        public async Task GetKedvencReceptByUserId1()
        {
            var felhasznaloId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Receptek = await _receptRepository.GetKedvencReceptByUserIdAsync(felhasznaloId);
        }

        public bool IsReceptKedvelt(int receptId, string felhasznaloId)
        {
            // Ellenõrizzük, hogy a KedvencRecept táblában található-e adott recept és felhasználói azonosítóval
            return _dbContext.KedvencRecept.Any(k => k.ReceptId == receptId && k.UserId == felhasznaloId);
        }

    }
}
