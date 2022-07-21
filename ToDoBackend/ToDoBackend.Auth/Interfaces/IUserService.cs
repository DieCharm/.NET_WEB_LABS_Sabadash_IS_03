using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ToDoBackend.Auth.Interfaces
{
    public interface IUserService
    {
        Task<IdentityUser> GetUserByIdAsync(string id);
        Task<bool> MakeUserAdminAsync(IdentityUser user);
    }
}