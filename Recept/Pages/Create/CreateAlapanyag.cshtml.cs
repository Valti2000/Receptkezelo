using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Pages.Create
{
    [Authorize(Roles = "Admin, ReceptIro")]
    public class CreateAlapanyagModel : PageModel
    {
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly IKategoriaRepository _kategoriaRepository;
        private readonly IAllergenRepository _allergenRepository;
        private readonly IAlapanyagAllergenRepository _alapanyagAllergenRepository;

        [BindProperty]
        public Alapanyag Alapanyag { get; set; } = new Alapanyag();

        public List<Kategorium> KategoriaLista { get; set; } = new List<Kategorium>();

        [BindProperty]
        public string SelectedAllergenIds { get; set; } = string.Empty;

        public List<Allergen> AllergenLista { get; set; } = new List<Allergen>();

        [BindProperty]
        public AlapanyagAllergen AlapanyagAllergen { get; set; } = new AlapanyagAllergen();

        public CreateAlapanyagModel(IAlapanyagRepository alapanyagRepository, IKategoriaRepository kategoriaRepository, IAllergenRepository allergenRepository, IAlapanyagAllergenRepository alapanyagAllergenRepository)
        {
            _alapanyagRepository = alapanyagRepository;
            _kategoriaRepository = kategoriaRepository;
            _allergenRepository = allergenRepository;
            _alapanyagAllergenRepository = alapanyagAllergenRepository;
        }

        public async Task OnGetAsync()
        {
            KategoriaLista = await _kategoriaRepository.GetAllAsync();
            AllergenLista = await _allergenRepository.GetAllAsync();
            SelectedAllergenIds = string.Empty;
        }
     
        public async Task<IActionResult> OnPostAsync()
        {

            if (Alapanyag.KategoriaId == 0)
            {
                ModelState.AddModelError("Alapanyag.KategoriaId", "Kérlek válassz egy kategóriát.");
                return Page();
            }

            Alapanyag.Kategoria = await _kategoriaRepository.GetByIdAsync(Alapanyag.KategoriaId);
            await _alapanyagRepository.CreateAsync(Alapanyag);


            if (SelectedAllergenIds != string.Empty)
            {

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
            return RedirectToPage("/Read/Alapanyagok");
        }
    }
}