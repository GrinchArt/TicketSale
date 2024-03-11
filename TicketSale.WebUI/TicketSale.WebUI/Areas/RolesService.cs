using Microsoft.AspNetCore.Identity;
using TicketSale.Domain;

namespace TicketSale.WebUI.Areas
{
    public class RolesService
    {
        public static async Task InitializeAsync(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User", "Manager" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
