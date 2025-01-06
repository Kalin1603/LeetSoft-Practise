using Microsoft.AspNetCore.Identity;

namespace Event_Management.Data
{
    public class SeedRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" }; 

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }

}
