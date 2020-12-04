using Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionCars.DB
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "Admin";
            string adminEmail = "admin@gmail.com";
            string password = "7751876";
            if(await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if(await roleManager.FindByNameAsync("guest") == null)
                {
                await roleManager.CreateAsync(new IdentityRole("guest"));
            }

            if(await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminName, Registration = DateTime.Now.ToUniversalTime() };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if(result.Succeeded)
                {
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.ConfirmEmailAsync(admin, code);
                }
            }
        }
    }
}
