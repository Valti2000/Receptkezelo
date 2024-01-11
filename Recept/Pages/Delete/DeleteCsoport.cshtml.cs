using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Delete
{
    [Authorize(Roles = "Admin")]
    public class DeleteCsoportModel : PageModel
    {
        private readonly ICsoportRepository _csoportRepository;

        public DeleteCsoportModel(ICsoportRepository csoportRepository)
        {
            _csoportRepository = csoportRepository;
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Csoport Csoport { get; set; } = new Csoport();

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
            if (Csoport == null)
            {
                return NotFound();
            }

            Csoport = await _csoportRepository.GetByIdAsync(Id);
            if (Csoport != null)
            {
                Csoport.Deleted = true;
                await _csoportRepository.UpdateAsync(Csoport);
            }

            return RedirectToPage("/Read/Csoportok");
        }
    }
}

