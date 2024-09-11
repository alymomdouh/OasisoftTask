using Microsoft.AspNetCore.Identity;
using OasisoftTask.Core.DomainModels;

namespace OasisoftTask.Common
{
    public static class DefaultSeeding
    {
        public static async Task SeedSuperAdminRole(RoleManager<ApplicationRole> _roleManager)
        {

            var roles = await _roleManager.FindByNameAsync(Constants.SuperAdminRole);
            if (roles == null)
            {

                var role = new ApplicationRole()
                {
                    Name = Constants.SuperAdminRole,
                    NormalizedName = Constants.SuperAdminRole
                };
                await _roleManager.CreateAsync(role);
            }
        }
        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> _userManager)
        {
            var userAdmin = await _userManager.GetUsersInRoleAsync(Constants.SuperAdminRole);

            if (!userAdmin.Any())
            {
                ApplicationUser user = new ApplicationUser("superadmin@admin.com", "superadmin", "superadmin");

                await _userManager.CreateAsync(user, "aly@12345");
                await _userManager.AddToRolesAsync(user, new List<string> { Constants.SuperAdminRole });
            }
        }
    }
}
