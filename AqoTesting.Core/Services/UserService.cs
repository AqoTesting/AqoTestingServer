using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AqoTesting.Shared.Infrastructure;
using System;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        IUserRespository _userRepository;

        public UserService(IUserRespository userRespository) {
            _userRepository = userRespository;
        }

        public async Task<User> GetUserByAuthData(SignInUserDTO authData) {
            return await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));
        }

        public async Task<User> GetUserByLogin(string login) {
            return await _userRepository.GetUserByLogin(login);
        }

        public async Task<User> GetUserByEmail(string email) {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<ObjectId> InsertUser(User user) {
            return await _userRepository.InsertUser(user);
        }

        public AuthorizedUserDTO GetAuthorizedUser(User user) {

            var authUser = new AuthUser {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Role = Role.User
            };

            var identity = GetIdentity(authUser);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthorizedUserDTO { 
                Token = encodedJwt,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email
            };
        }

        private ClaimsIdentity GetIdentity(AuthUser user)
        {
            
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Login", user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
                
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                
            return claimsIdentity;
        }
    }
}
