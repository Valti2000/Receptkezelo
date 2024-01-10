using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Recept.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public string Nev { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        [BindProperty]
        public bool RememberMe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Nev, Password, RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return RedirectToPage("/HomePage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sikertelen bejelentkezés."); 
                }
            }

            return Page();
        }
    }
}