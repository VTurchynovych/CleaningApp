using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using CleaningApp.Models;

namespace CleaningApp.Data
{
    public static class SeedRolesAndAdmin
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Employee", "User" };

            // Tworzenie ról, jeśli nie istnieją
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tworzenie admina
            string adminFullName = "Vladyslav Turchynovych";
            string adminEmail = "admin@cleaningapp.pl";
            string adminPassword = "Admin123!";
            string adminPhoneNumber = "+48 792 679 729";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    FullName = adminFullName,
                    Email = adminEmail,
                    PhoneNumber = adminPhoneNumber,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
