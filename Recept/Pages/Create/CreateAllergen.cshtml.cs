using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Recept.Data;
using Recept.Entity.Generated;
using Recept.Services;

namespace Recept.Pages.Create
{
    [Authorize(Roles = "Admin, ReceptIro")]
    public class CreateAllergenModel : PageModel
    {

        private readonly ReceptekContext _context;

        [BindProperty]
        public Allergen Allergen { get; set; } = new Allergen();

        public CreateAllergenModel(ReceptekContext context)
        {
            _context = context;
        }


        public void OnGet()
        {
            // Itt lehet inicializációt végezni, ha szükséges
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Allergenek.Add(Allergen);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Read/Allergenek");
        }
    }



}
