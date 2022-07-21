using System;
using Microsoft.AspNetCore.Identity;

namespace ToDoBackend.Server
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleSeeder(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedRolesAsync()
        {
            if (!_roleManager.RoleExistsAsync("admin").Result)
            {
                try
                {
                    IdentityResult result = _roleManager.CreateAsync(new IdentityRole("admin")).Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void SeedAdminAsync()
        {
            IdentityUser admin = _userManager.FindByEmailAsync("dymedroll228@gmail.com").Result;

            if (admin != null && !_userManager.IsInRoleAsync(admin, "admin").Result)
            {
                try
                {
                    IdentityResult result = _userManager.AddToRoleAsync(admin, "admin").Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}