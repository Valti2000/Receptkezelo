using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;
using Recept.Entity.Generated;
using Recept.Repositories;
using Recept.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Recept.Data;

namespace Recept.Pages.Read
{
    public class AlapanyagokModel : PageModel
    {
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly IKategoriaRepository _kategoriaRepository;
        private readonly IAllergenRepository _allergenRepository;
        private readonly IAlapanyagAllergenRepository _alapanyagAllergenRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly ReceptekContext _dbContext;

        public AlapanyagokModel(IAlapanyagRepository alapanyagRepository, IKategoriaRepository kategoriaRepository, IAllergenRepository allergenRepository,IAlapanyagAllergenRepository alapanyagAllergenRepository, UnitOfWork unitOfWork, ReceptekContext dbContext)
        {
            _alapanyagRepository = alapanyagRepository;
            _kategoriaRepository = kategoriaRepository;
            _allergenRepository = allergenRepository;
            _alapanyagAllergenRepository = alapanyagAllergenRepository;
            _unitOfWork = unitOfWork;
            Alapanyagok = new List<Alapanyag>();
            _dbContext = dbContext;
            
    }
        public IEnumerable<Alapanyag> Alapanyagok { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }

        public async Task OnGetAsync()
        {
            Alapanyagok = await _alapanyagRepository.GetAllAsync();
        }

        public async Task OnPostAsync()
        {
            if (IsDeleted)
            {
                Alapanyagok = await _dbContext.Alapanyagok.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                Alapanyagok = await _alapanyagRepository.GetAllAsync();
            }
        }

        public async Task<string?> GetKategoriaNevById(int kategoriaId)
        {
            var kategoria = await _kategoriaRepository.GetByIdAsync(kategoriaId);
            return kategoria?.Nev;
        }

        public async Task<List<Allergen>> GetAllergenByAlapanyagIdAsync(int alapanyagId)
        {
            return await _alapanyagRepository.GetAllergenByAlapanyagIdAsync(alapanyagId);
        }
    }
}
