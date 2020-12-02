using Microsoft.IdentityModel.Tokens;
using System.Text;

#pragma warning disable CS8618
namespace AqoTesting.Shared.Infrastructure
{
    public class AuthOptionsConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key {
            get => null;
            set => SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(value));
        }
        public int LifeTime { get; set; }
        public SymmetricSecurityKey SymmetricSecurityKey { get; set; }
    }
}
