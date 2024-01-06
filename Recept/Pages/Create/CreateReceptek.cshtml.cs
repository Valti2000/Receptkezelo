using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recept.Entity.Generated;
using Recept.Pages.Read;
using Recept.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recept.Pages.Create
{
    public class CreateReceptModel : PageModel
    {
        private readonly IReceptRepository _receptRepository;
        private readonly IHozzavaloRepository _hozzavaloRepository;
        private readonly IReceptHozzavaloRepository _receptHozzavaloRepository;

        [BindProperty]
        public ReceptHozzavalo ReceptHozzavalo { get; set; } = new ReceptHozzavalo();

        public List<Hozzavalo> HozzavaloLista { get; set; } = new List<Hozzavalo>();

        public Receptek Receptek { get; set; } = new Receptek();

        [BindProperty]
        public string SelectedHozzavaloIds { get; set; } = string.Empty;


        public CreateReceptModel(IReceptRepository receptRepository, IHozzavaloRepository hozzavaloRepository, IReceptHozzavaloRepository receptHozzavaloRepository)
        {
            _receptRepository = receptRepository;
            _hozzavaloRepository = hozzavaloRepository;
            _receptHozzavaloRepository = receptHozzavaloRepository;
        }

        public async Task OnGetAsync()
        {
            HozzavaloLista = await _hozzavaloRepository.GetAllAsync();
            SelectedHozzavaloIds = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var newRecept = new Receptek
            {
                Cim = ReceptHozzavalo.Recept.Cim,
                Leiras = ReceptHozzavalo.Recept.Leiras,
                ElokeszitesiIdo = ReceptHozzavalo.Recept.ElokeszitesiIdo,
                FozesiIdo = ReceptHozzavalo.Recept.FozesiIdo
            };

            if (string.IsNullOrWhiteSpace(SelectedHozzavaloIds))
            {
                TempData["ErrorMessage"] = "Válassz legalább egy hozzávalót!";
                return RedirectToPage("/Create/CreateReceptek");
            }


                await _receptRepository.AddAsync(newRecept);

                List<int> selectedIds = SelectedHozzavaloIds.Split(',').Select(int.Parse).ToList();

                foreach (var hozzavaloId in selectedIds)
                {
                    var receptHozzavalo = new ReceptHozzavalo
                    {
                        ReceptId = newRecept.Id,
                        HozzavaloId = hozzavaloId
                    };

                    await _receptHozzavaloRepository.AddAsync(receptHozzavalo);
                }

            return RedirectToPage("/Read/Receptek");
        }
    }
}
