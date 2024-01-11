using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Recept.Data;

namespace Recept.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ReceptekContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ReceptekContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _userManager = userManager;
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
                var user = await _userManager.FindByNameAsync(Nev);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(Nev, Password, RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
 
                        var roles = await _userManager.GetRolesAsync(user);
                        var role = roles.FirstOrDefault() ?? "ReceptOlvaso";
                        var token = JwtTokenGenerator.GenerateJwtToken(Nev, role);

                       
                        Console.WriteLine($"Sikeres bejelentkezés! Generált JWT token: {token}");

 
                        Response.Cookies.Append("jwt", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict

                        });

                        return RedirectToPage("/Homepage"); 
                    }
                }
            }

                    return Page();
        }
    }
}
