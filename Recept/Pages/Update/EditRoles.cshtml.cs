using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Recept.Pages.Update
{
    [Authorize(Roles = "Admin")]
    public class EditRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class UserModel
        {
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public List<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();

        public List<UserModel> Users { get; set; } = new List<UserModel>();

        [BindProperty]
        public List<string> SelectedRoles { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                Users.Add(new UserModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Role = userRole.FirstOrDefault() ?? string.Empty

                });
            }

            AvailableRoles = _roleManager.Roles
            .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
            .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId, string selectedRole)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);

            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            await _userManager.RemoveFromRolesAsync(currentUser, currentRoles);

            var result = await _userManager.AddToRoleAsync(currentUser, selectedRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToPage("/Update/EditRoles");
        }
    }
}

