using Microsoft.AspNetCore.Identity;
using MyBookCollection.Helpers.Roles;
using MyBookCollection.Models.Data;

namespace MyBookCollection.Models
{
    public static class DbInitializer
    {
        public static async Task SeedDefaultUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                //Simple user related data
                var userRole = Role.User;
                var userEmail = "user@gmail.com";

                //Create role if doesn't exist:
                if(!await roleManager.RoleExistsAsync(userRole))
                {
                    await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = userRole
                    });
                }


                // Seed Default User
                if (await userManager.FindByEmailAsync(userEmail) == null) 
                {
                    var simpleUser = new AppUser()
                    {
                        FullName = "Simple User",
                        UserName = "simple-user",
                        Email = userEmail,
                        EmailConfirmed = true
                    };
                    // Set default password
                    await userManager.CreateAsync(simpleUser, "User@1234");
                    //Assign role
                    await userManager.AddToRoleAsync(simpleUser, userRole);
                }

                //Admin data
                var roleAdmin = Role.Admin;
                var adminUserEmail = "sssk08844@gmail.com";

                //Create role if doesn't exist:
                if(!await roleManager.RoleExistsAsync(roleAdmin))
                {
                    await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = roleAdmin
                    });
                }

                // Seed defsult admin
                if (await userManager.FindByEmailAsync(adminUserEmail) == null)
                {
                    var adminUser = new AppUser()
                    {
                        FullName = "Sourav Kundu",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };

                    //Set Password
                    await userManager.CreateAsync(adminUser, "Admin@1234");

                    // Assign Role
                    await userManager.AddToRoleAsync(adminUser, roleAdmin);
                }
            }

        }
    }
}
