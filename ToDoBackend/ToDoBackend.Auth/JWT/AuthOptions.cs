using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ToDoBackend.Auth.JWT
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";
        public const string AUDIENCE = "ResourceServer";
        public const string KEY = "letkeybehere12334";
        public const int LIFETIME = 3600;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}