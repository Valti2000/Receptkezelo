// Recept.Pages.AllergenekModel osztály
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Pages
{
    public class AllergenekModel : PageModel
    {
        private readonly IAllergenRepository _allergenRepository;
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly ReceptekContext _dbContext;
        public AllergenekModel(IAllergenRepository allergenRepository, IAlapanyagRepository alapanyagRepository, ReceptekContext dbContext)
        {
            _allergenRepository = allergenRepository;
            _alapanyagRepository = alapanyagRepository;
            Allergenek = new List<Allergen>();
            _dbContext = dbContext;
        }

        public IEnumerable<Allergen> Allergenek { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }

        public async Task OnGetAsync()
        {
            Allergenek = await _allergenRepository.GetAllAsync();
        }

        public async Task OnPostAsync()
        {
            if (IsDeleted)
            {
                Allergenek = await _dbContext.Allergenek.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                Allergenek = await _allergenRepository.GetAllAsync();
            }
        }


        public async Task<List<Alapanyag>> AlapanyagokByAllergenIdAsync(int allergenId)
        {
            var alapanyagok = await _alapanyagRepository.GetAlapanyagokByAllergenIdAsync(allergenId);

            return alapanyagok;
        }
    }
}
