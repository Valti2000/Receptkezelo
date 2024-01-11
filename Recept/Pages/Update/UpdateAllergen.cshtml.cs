// UpdateAllergenModel.cs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Update
{
    [Authorize(Roles = "Admin")]
    public class UpdateAllergenModel : PageModel
    {
        private readonly IAllergenRepository _allergenRepository;

        [BindProperty]
        public Allergen Allergen { get; set; } = new Allergen();

        public UpdateAllergenModel(IAllergenRepository allergenRepository)
        {
            _allergenRepository = allergenRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Allergen = await _allergenRepository.GetByIdAsync(id);

            if (Allergen == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _allergenRepository.UpdateAsync(Allergen);

            return RedirectToPage("/Read/Allergenek");
        }
    }
}
