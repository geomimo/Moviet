using Microsoft.AspNetCore.Identity;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet
{
    public static class SeedData
    {
        public static void Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);          
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(userManager.FindByNameAsync(Roles.Administrator).Result == null)
            {
                var user = new IdentityUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };

                var result = userManager.CreateAsync(user, "admin").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Roles.Rater).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.Rater
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Roles.Registrator).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.Registrator
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Roles.Administrator).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.Administrator
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
