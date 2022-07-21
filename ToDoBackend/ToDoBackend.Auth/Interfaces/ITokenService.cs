using Microsoft.AspNetCore.Identity;

namespace ToDoBackend.Auth.Interfaces
{
    public interface ITokenService
    {
        string GetToken(IdentityUser user);
    }
}