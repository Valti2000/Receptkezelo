using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Update
{
    public class UpdateKategoriaModel : PageModel
    {
        private readonly IKategoriaRepository _kategoriaRepository;

        public UpdateKategoriaModel(IKategoriaRepository kategoriaRepository)
        {
            _kategoriaRepository = kategoriaRepository;
        }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _kategoriaRepository.UpdateAsync(Kategoria);

            return RedirectToPage("/Read/Kategoriak");
        }
    }
}
