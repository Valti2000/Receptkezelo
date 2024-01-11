using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Threading.Tasks;

namespace Recept.Pages.Create
{
    [Authorize(Roles = "Admin, ReceptIro")]
    public class CreateCsoportModel : PageModel
    {
        private readonly ICsoportRepository _csoportRepository;

        public CreateCsoportModel(ICsoportRepository csoportRepository)
        {
            _csoportRepository = csoportRepository;
        }

        [BindProperty]
        public Csoport Csoport { get; set; } = new Csoport();

        public async Task<IActionResult> OnPostAsync()
        {
            await _csoportRepository.AddAsync(Csoport);
            return RedirectToPage("/Read/Csoportok");
        }
    }
}
