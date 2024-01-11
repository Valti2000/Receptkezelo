using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Recept.Entity.Generated;
using Recept.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Recept.Pages.Delete
{
    [Authorize(Roles = "Admin")]
    public class DeleteAlapanyagModel : PageModel
    {
        private readonly IAlapanyagRepository _alapanyagRepository;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Alapanyag Alapanyag { get; set; } = new Alapanyag();

        public DeleteAlapanyagModel(IAlapanyagRepository alapanyagRepository)
        {
            _alapanyagRepository = alapanyagRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Alapanyag = await _alapanyagRepository.GetByIdAsync(id.Value);

            if (Alapanyag == null)
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

            Alapanyag = await _alapanyagRepository.GetByIdAsync(Id);

            if (Alapanyag != null)
            {

                var nincsFuggoseg = await _alapanyagRepository.VanFuggosegAsync(Id);

                if (!nincsFuggoseg)
                {

                    Alapanyag.Deleted = true;
                    await _alapanyagRepository.UpdateAsync(Alapanyag);

                    TempData["Message"] = "Az Alapanyag sikeresen törölve lett.";
                    return Page();

                }
                else
                {
                    TempData["ErrorMessage"] = "Az Alapanyag nem lett törölve függõség miatt.";
                    return Page();
                }
            }

            return RedirectToPage("/Read/Alapanyagok");
        }
    }
}
