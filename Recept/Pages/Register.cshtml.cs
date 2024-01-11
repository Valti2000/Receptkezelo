using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Recept.Services;
using Microsoft.AspNetCore.Identity;

namespace Recept.Pages
{
public class RegisterModel : PageModel
{
    private readonly IRegistrationService _registrationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(IRegistrationService registrationService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _registrationService = registrationService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
    public RegisterViewModel Input { get; set; } = new RegisterViewModel();

    public class RegisterViewModel
    {
        [Required]
        public string Nev { get; set; } = null!;

        [Required]
        public string Orszag { get; set; } = null!;

        [Required]
        public string Varos { get; set; } = null!;

        [Required]
        [DataType(DataType.Url)]
        public string ProfilePictureUrl { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public void OnGet()
    {
    }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _registrationService.RegisterUser(Input);

                if (result)
                {
                    var user = await _userManager.FindByNameAsync(Input.Nev);

                    var roleResult = await _userManager.AddToRoleAsync(user, "ReceptOlvaso");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToPage("/Login");
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }


            return Page();
        }
    }
}