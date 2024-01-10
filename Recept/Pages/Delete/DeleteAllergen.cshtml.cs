using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Repositories;
using Recept.Entity.Generated;

namespace Recept.Pages.Delete
{
    public class DeleteAllergenModel : PageModel
    {
        private readonly IAllergenRepository _allergenRepository;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Allergen Allergen { get; set; } = new Allergen();

        public DeleteAllergenModel(IAllergenRepository allergenRepository)
        {
            _allergenRepository = allergenRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Itt ellenõrizzük, hogy az allergen létezik-e
            var allergen = await _allergenRepository.GetByIdAsync(id.Value);
            if (allergen == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Id == 0)
            {
                return NotFound();
            }

            Allergen = await _allergenRepository.GetByIdAsync(Id);
            if (Allergen != null)
            {
               
                

                var nincsFuggoseg = await _allergenRepository.VanFuggosegAsync(Id);

                if (!nincsFuggoseg)
                {

                    Allergen.Deleted = true;
                    await _allergenRepository.UpdateAsync(Allergen);

                    TempData["Message"] = "Az Allergen sikeresen törölve lett.";
                    return Page();

                }
                else
                {
                    TempData["ErrorMessage"] = "Az Allergen nem lett törölve függõség miatt.";
                    return Page();
                }
            }

            return RedirectToPage("/Read/Allergenek");
        }
    }
}
