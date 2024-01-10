using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Pages.Create;
using Recept.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Recept.Pages.Update
{
    public class UpdateAlapanyagModel : PageModel
    {
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly IKategoriaRepository _kategoriaRepository;
        private readonly IAllergenRepository _allergenRepository;
        private readonly IAlapanyagAllergenRepository _alapanyagAllergenRepository;

        [BindProperty]
        public string SelectedAllergenIds { get; set; } = string.Empty;
        public List<Kategorium> KategoriaLista { get; set; } = new List<Kategorium>();
        public List<Allergen> AllergenLista { get; set; } = new List<Allergen>();
        public AlapanyagAllergen AlapanyagAllergen { get; set; } = new AlapanyagAllergen();

        [BindProperty]
        public Alapanyag Alapanyag { get; set; } = new Alapanyag();

        [BindProperty]
        public int SelectedKategoriaId { get; set; }

        public UpdateAlapanyagModel(IAlapanyagRepository alapanyagRepository, IKategoriaRepository kategoriaRepository, IAllergenRepository allergenRepository, IAlapanyagAllergenRepository alapanyagAllergenRepository)
        {
            _alapanyagRepository = alapanyagRepository;
            _kategoriaRepository = kategoriaRepository;
            _allergenRepository = allergenRepository;
            _alapanyagAllergenRepository = alapanyagAllergenRepository;
        }
            
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            KategoriaLista = await _kategoriaRepository.GetAllAsync();
            AllergenLista = await _allergenRepository.GetAllAsync();
            SelectedAllergenIds = string.Empty;

            if (id == null)
            {
                return NotFound();
            }

            Alapanyag = await _alapanyagRepository.GetByIdAsync(id.Value);
            SelectedKategoriaId = Alapanyag.KategoriaId;

            Alapanyag = await _alapanyagRepository.GetByIdAsync(id.Value);

            if (Alapanyag == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            Alapanyag.KategoriaId = SelectedKategoriaId;

            if (SelectedAllergenIds != string.Empty)
            {

                await _alapanyagAllergenRepository.DeleteAllByAlapanyagIdAsync(Alapanyag.Id);

                List<int> selectedIds = SelectedAllergenIds.Split(',').Select(int.Parse).ToList();

                foreach (var allergenId in selectedIds)
                {
                    var alapanyagAllergen = new AlapanyagAllergen
                    {
                        AlapanyagId = Alapanyag.Id,
                        AllergenId = allergenId
                    };
                    await _alapanyagAllergenRepository.CreateAsync(alapanyagAllergen);
                }
            }

            await _alapanyagRepository.UpdateAsync(Alapanyag);
            return RedirectToPage("/Read/Alapanyagok");
        }
    }
}