using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.BD.Users;
// using AqoTesting.Domain.Controllers;
// Эта жопа не подключается
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using AqoTesting.Shared.Enums;
using System.Net.Security;
using System.Net;

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

        public async Task<AuthorizedUserDTO> SignIn(string login, string password)
        {
            // Думаю, всё-таки надо вынести в отдельную функцию, но я хз, куда, так шо вот
            byte[] bytedPassword = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash;
            using(SHA256 sha256 = SHA256.Create()) { passwordHash = sha256.ComputeHash(bytedPassword); }

            bool signedIn = true;
            // signedIn = MongoIOController.SignIn(login, passwordHash); // Жопа не подключается
            User user; // Это надо получить из базы, если успешная авторизация

            if(signedIn)
            {
                string token = " generateJwtToken() ";

                return new AuthorizedUserDTO
                {
                    Token = token,
                    //Login = user.Login,
                    //Email = user.Email,
                    //Name  = user.Name
                };
            } else
            {
                // Ашипка, а как её - хз
                // Ну типа OperationErrorMessages.WrongAuthData, это я создал, но как его вернуть
                return new AuthorizedUserDTO {};
            }
        }
    }
}
