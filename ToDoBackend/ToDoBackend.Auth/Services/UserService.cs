using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ToDoBackend.Auth.Interfaces;

namespace ToDoBackend.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> MakeUserAdminAsync(IdentityUser user)
        {
            try
            {
                return (await _userManager.AddToRoleAsync(user, "admin")).Succeeded;
            }
            catch
            {
                return false;
            }
        }
    }
}