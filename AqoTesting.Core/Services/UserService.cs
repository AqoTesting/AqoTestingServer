using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
    }
}
