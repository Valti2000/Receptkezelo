using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Delete
{
    [Authorize(Roles = "Admin")]
    public class DeleteHozzavaloModel : PageModel
    {
        private readonly IHozzavaloRepository _hozzavaloRepository;

        public DeleteHozzavaloModel(IHozzavaloRepository hozzavaloRepository)
        {
            _hozzavaloRepository = hozzavaloRepository;
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Hozzavalo Hozzavalo { get; set; } = new Hozzavalo();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hozzavalo = await _hozzavaloRepository.GetByIdAsync(id.Value);

            if (Hozzavalo == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Hozzavalo = await _hozzavaloRepository.GetByIdAsync(Id);


            if (Hozzavalo != null)
            {
                var nincsFuggoseg = await _hozzavaloRepository.VanFuggosegAsync(Id);

                if (!nincsFuggoseg)
                {

                    Hozzavalo.Deleted = true;
                    await _hozzavaloRepository.UpdateAsync(Hozzavalo);

                    TempData["Message"] = "Az Allergen sikeresen törölve lett.";
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = "Az Allergen nem lett törölve függõség miatt.";
                    return Page();
                }

            }

            return RedirectToPage("/Read/Hozzavalok");
        }
    }
}





