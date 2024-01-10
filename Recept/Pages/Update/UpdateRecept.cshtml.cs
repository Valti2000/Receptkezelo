using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Recept.Pages.Update
{
    public class UpdateReceptModel : PageModel
    {
        private readonly IReceptRepository _receptRepository;
        private readonly IHozzavaloRepository _hozzavaloRepository;
        private readonly IReceptHozzavaloRepository _receptHozzavaloRepository;

        [BindProperty]
        public Receptek Recept { get; set; } = new Receptek();

        public ReceptHozzavalo ReceptHozzavalo { get; set; } = new ReceptHozzavalo();

        [BindProperty]
        [Required(AllowEmptyStrings = true)]
        public string SelectedHozzavaloIds { get; set; } = string.Empty;

        public List<Hozzavalo> HozzavaloLista { get; set; } = new List<Hozzavalo>();


        public UpdateReceptModel(IReceptRepository receptRepository, IHozzavaloRepository hozzavaloRepository, IReceptHozzavaloRepository receptHozzavaloRepository)
        {
            _receptRepository = receptRepository;
            _hozzavaloRepository = hozzavaloRepository;
            _receptHozzavaloRepository = receptHozzavaloRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Recept = await _receptRepository.GetByIdAsync(id);

            if (Recept == null)
            {
                return NotFound();
            }

            HozzavaloLista = await _hozzavaloRepository.GetAllAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (SelectedHozzavaloIds != string.Empty)
            {
                List<int> selectedIds = SelectedHozzavaloIds.Split(',').Select(int.Parse).ToList();

                if (selectedIds != null)
                {
                    await _receptHozzavaloRepository.DeleteByReceptIdAsync(id);

                    foreach (var hozzavaloId in selectedIds)
                    {
                        var receptHozzavalo = new ReceptHozzavalo
                        {
                            ReceptId = Recept.Id,
                            HozzavaloId = hozzavaloId
                        };
                        await _receptHozzavaloRepository.AddAsync(receptHozzavalo);
                    }
                }
            }

            await _receptRepository.UpdateAsync(Recept);

            return RedirectToPage("/Read/Receptek");
        }
    }
}
