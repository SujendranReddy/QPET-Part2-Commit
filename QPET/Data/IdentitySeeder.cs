using Microsoft.AspNetCore.Identity;
using QPET.Models;

namespace QPET.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var userManager =
                scope.ServiceProvider
                    .GetRequiredService<UserManager<AdminUser>>();

            var roleManager =
                scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

            const string roleName = "Admin";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(roleName));
            }

            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];
            var fullName = configuration["AdminSeed:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new AdminUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName ?? "QPET Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result =
                    await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(errors);
                }
            }

            if (!await userManager.IsInRoleAsync(admin, roleName))
            {
                await userManager.AddToRoleAsync(admin, roleName);
            }
        }
    }
}