using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;


namespace Recept.Pages.Create
{
    public class CreateKategoriaModel : PageModel
    {
        private readonly IKategoriaRepository _kategoriaRepository;

        [BindProperty]
        public Kategorium Kategoria { get; set; } = new Kategorium();

        public CreateKategoriaModel(IKategoriaRepository kategoriaRepository)
        {
            _kategoriaRepository = kategoriaRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _kategoriaRepository.CreateAsync(Kategoria);

            return RedirectToPage("/Read/Kategoriak");
        }
    }
}
