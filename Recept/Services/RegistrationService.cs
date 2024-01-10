using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using static Recept.Pages.RegisterModel;


namespace Recept.Services
{
public interface IRegistrationService
{
    Task<bool> RegisterUser(RegisterViewModel model);
}

public class RegistrationService : IRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegistrationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;

        }

        public async Task<bool> RegisterUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Nev,
                Email = model.Nev,
                Nev = model.Nev,
                Varos = model.Varos,
                Orszag = model.Orszag,
                ProfilePictureUrl = model.ProfilePictureUrl
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return true;
            }

            return false;
        }
    }
}