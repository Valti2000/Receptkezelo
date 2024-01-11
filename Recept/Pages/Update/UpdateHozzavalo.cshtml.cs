using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Pages.Update
{
    [Authorize(Roles = "Admin")]
    public class UpdateHozzavaloModel : PageModel
    {
        private readonly IHozzavaloRepository _hozzavaloRepository;
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly ICsoportRepository _csoportRepository;

        public UpdateHozzavaloModel(IHozzavaloRepository hozzavaloRepository, IAlapanyagRepository alapanyagRepository, ICsoportRepository csoportRepository)
        {
            _hozzavaloRepository = hozzavaloRepository;
            _alapanyagRepository = alapanyagRepository;
            _csoportRepository = csoportRepository;
        }

        [BindProperty]
        public Hozzavalo Hozzavalo { get; set; } = new Hozzavalo(); 

        public List<Alapanyag> Alapanyagok { get; set; } = new List<Alapanyag>();
        public List<Csoport> Csoportok { get; set; } = new List<Csoport>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Hozzavalo = await _hozzavaloRepository.GetByIdAsync(id);

            if (Hozzavalo == null)
            {
                return NotFound();
            }

            Alapanyagok = await _alapanyagRepository.GetAllAsync();
            Csoportok = await _csoportRepository.GetAllAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Hozzavalo == null)
                {
                    return NotFound();
                }

                await _hozzavaloRepository.UpdateAsync(Hozzavalo);

                return RedirectToPage("/Read/Hozzavalok");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt az adatok frissítése közben: {ex.Message}");
                throw; // Esetleg kezeld vagy továbbítsd a kivételt aszerint, amire szükséged van.
            }
        }
    }
}
