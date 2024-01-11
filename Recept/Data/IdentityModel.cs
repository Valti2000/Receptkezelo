using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recept.Entity;
using Recept.Entity.Generated;


namespace Recept.Data
{
    public static class IdentityModel
    {
        public static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                var roles = new List<string> { "Admin", "ReceptIro", "ReceptOlvaso" };
                foreach (var roleName in roles)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var adminRole = await roleManager.FindByNameAsync("Admin");

                if (adminRole == null || (await userManager.GetUsersInRoleAsync("Admin")).Count == 0)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        Nev = "admin",
                        Varos = "admin",
                        Orszag = "admin",
                        ProfilePictureUrl = "https://admin/4xDEWgBd1S"
                    };

                    var result = await userManager.CreateAsync(adminUser, "admin");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
        }
    }
}
