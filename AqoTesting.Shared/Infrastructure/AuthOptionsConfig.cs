using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AqoTesting.Shared.Infrastructure
{
    public class AuthOptionsConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int LifeTime { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
