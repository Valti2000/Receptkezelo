using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Create
{
    public class CreateHozzavaloModel : PageModel
    {
        private readonly IHozzavaloRepository _hozzavaloRepository;

        private readonly IAlapanyagRepository _alapanyagRepository;

        private readonly ICsoportRepository _csoportRepository;
        public List<Alapanyag> AlapanyagLista { get; set; } = new List<Alapanyag>();
        public List<Csoport> CsoportLista { get; set; } = new List<Csoport>();

        public CreateHozzavaloModel(IHozzavaloRepository hozzavaloRepository, IAlapanyagRepository alapanyagRepository, ICsoportRepository csoportRepository)
        {
            _hozzavaloRepository = hozzavaloRepository;
            _alapanyagRepository = alapanyagRepository;
            _csoportRepository = csoportRepository;
        }

        public async Task OnGetAsync()
        {
            AlapanyagLista = await _alapanyagRepository.GetAllAsync();
            CsoportLista = await _csoportRepository.GetAllAsync();
        }

        [BindProperty]
        public Hozzavalo Hozzavalo { get; set; } = new Hozzavalo();

        public async Task<IActionResult> OnPostAsync()
        {

            await _hozzavaloRepository.CreateAsync(Hozzavalo);

            return RedirectToPage("/Read/Hozzavalok");
        }
    }
}
