using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Threading.Tasks;

namespace Recept.Pages.Delete
{
    public class DeleteReceptModel : PageModel
    {
        private readonly IReceptRepository _receptRepository;
        private readonly IReceptHozzavaloRepository _receptHozzavaloRepository;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public Receptek Receptek { get; set; } = new Receptek();

        public DeleteReceptModel(IReceptRepository receptRepository, IReceptHozzavaloRepository receptHozzavaloRepository)
        {
            _receptRepository = receptRepository;
            _receptHozzavaloRepository = receptHozzavaloRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Receptek = await _receptRepository.GetByIdAsync(id.Value);

            if (Receptek == null)
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

            Receptek = await _receptRepository.GetByIdAsync(Id);

            if (Receptek != null)
            {
                var nincsFuggoseg = await _receptRepository.VanFuggosegAsync(Id);

                if (!nincsFuggoseg)
                {
                    await _receptRepository.LogikaiTorlesReceptAsync(Id);

                    TempData["Message"] = "A recept sikeresen törölve lett.";
                    return Page();

                }
                else
                {
                    TempData["ErrorMessage"] = "A recept nem lett törölve függõség miatt.";
                    return Page();


                }
            }
            return RedirectToPage("/Read/Receptek");
        }
    }
}
