using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AqoTesting.Shared.Infrastructure
{
    public static class AuthOptions
    {
        public const string ISSUER = "AqoTestingAuthServer"; // издатель токена
        public const string AUDIENCE = "https://localhost:5001/"; // потребитель токена
        const string KEY = "EAaHxzvgoMjbwjcPycvtB7d5hNPIe89x";   // ключ для шифрации
        public const int LIFETIME = 1209600; // время жизни токена - минут (14 дней)
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
