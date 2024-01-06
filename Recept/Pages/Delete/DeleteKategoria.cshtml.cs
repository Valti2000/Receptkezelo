using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Delete
{
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
                Kategoria.Deleted = true;
                await _kategoriaRepository.UpdateAsync(Kategoria);
            }

            return RedirectToPage("/Read/Kategoriak");
        }
    }
}
