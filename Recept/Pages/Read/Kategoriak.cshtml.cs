using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Repositories;

namespace Recept.Pages.Read
{
    public class KategoriakModel : PageModel
    {
        private readonly IKategoriaRepository _kategoriaRepository;
        private readonly IAlapanyagRepository _alapanyagRepository;
        private readonly ReceptekContext _dbContext;

        public KategoriakModel(IKategoriaRepository kategoriaRepository, IAlapanyagRepository alapanyagRepository, ReceptekContext dbContext)
        {
            _kategoriaRepository = kategoriaRepository;
            _alapanyagRepository = alapanyagRepository;
            Kategoriak = new List<Kategorium>();
            _dbContext = dbContext;
        }

        public IEnumerable<Kategorium> Kategoriak { get; set; }

        public async Task OnGetAsync()
        {
            Kategoriak = await _kategoriaRepository.GetAllAsync();
        }

        [BindProperty(SupportsGet = true)]
        public bool IsDeleted { get; set; }

        public async Task OnPostAsync()
        {
            if (IsDeleted)
            {
                Kategoriak = await _dbContext.Kategoria.IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                Kategoriak = await _kategoriaRepository.GetAllAsync();
            }
        }

        public async Task<List<Alapanyag>> AlapanyagokByKategoriaIdAsync(int kategoriaId)
        {
            var alapanyagIds = await _alapanyagRepository.GetAlapanyagIdByKategoriaIdAsync(kategoriaId);
            var alapanyagok = await _alapanyagRepository.GetAlapanyagokByIdsAsync(alapanyagIds);

            return alapanyagok;
        }
    }
}
