using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Recept.Services;

namespace Recept.Pages
{
public class RegisterModel : PageModel
{
    private readonly IRegistrationService _registrationService;

    public RegisterModel(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
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
                // Sikeres regisztr�ci� eset�n tov�bbi teend�k
                return RedirectToPage("/Login");
            }
            else
            {
                // Sikertelen regisztr�ci� eset�n kezel�s
                // Pl.: ModelState hiba�zenetek hozz�ad�sa
            }
        }

        // Hiba eset�n visszat�r�s az oldalra
        return Page();
    }
}
}