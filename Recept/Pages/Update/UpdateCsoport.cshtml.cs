using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Update
{
    public class UpdateCsoportModel : PageModel
    {
        private readonly ICsoportRepository _csoportRepository;

        public UpdateCsoportModel(ICsoportRepository csoportRepository)
        {
            _csoportRepository = csoportRepository;
        }

        [BindProperty]
        public Csoport Csoport { get; set; }  = new Csoport();

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Csoport = await _csoportRepository.GetByIdAsync(id);

            if (Csoport == null)
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

            await _csoportRepository.UpdateAsync(Csoport);

            return RedirectToPage("/Read/Csoportok");
        }
    }
}
