using Microsoft.IdentityModel.Tokens;
using System.Text;

#pragma warning disable CS8618
namespace AqoTesting.Shared.Infrastructure
{
    public class AuthOptionsConfig
    {
        private SymmetricSecurityKey _key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key {
            set => _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(value));
        }
        public int LifeTime { get; set; }
        public SymmetricSecurityKey SymmetricSecurityKey {
            get => _key;
        }
    }
}
