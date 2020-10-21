using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.BD.Users;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Core.Utils;

namespace AqoTesting.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            /*Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }*/

            // если пользователя не найдено
            return null;
        }

        public async Task<User> GetUserByAuthData(LoginUserDTO authData) {
            return MongoIOController.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));
        }

        public async Task<string> GenerateJwtToken(User user) {
            return "token";
        }
    }
}
