using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Delete
{
    [Authorize(Roles = "Admin")]
    public class DeleteKategoriaModel : PageModel
    {
        private readonly IKategoriaRepository _kategoriaRepository;

        public DeleteKategoriaModel(IKategoriaRepository kategoriaRepository)
        {
            _kategoriaRepository = kategoriaRepository;
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Kategorium Kategoria { get; set; } = new Kategorium();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Kategoria = await _kategoriaRepository.GetByIdAsync(id.Value);

            if (Kategoria == null)
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

            Kategoria = await _kategoriaRepository.GetByIdAsync(Id);

            if (Kategoria != null)
            {
                var nincsFuggoseg = await _kategoriaRepository.VanFuggosegAsync(Id);

                if (!nincsFuggoseg)
                {

                    Kategoria.Deleted = true;
                    await _kategoriaRepository.UpdateAsync(Kategoria);

                    TempData["Message"] = "A Ketegoria sikeresen törölve lett.";
                    return Page();

                }
                else
                {
                    TempData["ErrorMessage"] = "A Ketegoria nem lett törölve függõség miatt.";
                    return Page();
                }
            }

            return RedirectToPage("/Read/Kategoriak");
        }
    }
}



