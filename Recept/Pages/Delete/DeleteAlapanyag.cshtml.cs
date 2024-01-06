using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Delete
{
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
                Alapanyag.Deleted = true;
                await _alapanyagRepository.UpdateAsync(Alapanyag);
            }

            return RedirectToPage("/Read/Alapanyagok");
        }
    }
}
