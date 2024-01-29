using AccountService.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AccountService.Identity
{
    public class IdentitySeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var user = new AppUser
            {
                DisplayName = "admin",
                Email = "admin3@ecommerce.com",
                UserName = "admin3@ecommerce.com",
                PhoneNumber = "1234567890"
            };
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Dhruv@22102001");
                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    var role = new IdentityRole();
                    role.Name = "admin";
                    await roleManager.CreateAsync(role);
                }
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
