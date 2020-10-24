using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Security.Claims;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AqoTesting.Shared.Infrastructure;
using System;
using MongoDB.Bson;
using AutoMapper;

namespace AqoTesting.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        IUserRespository _userRepository;

        public UserService(IUserRespository userRespository)
        {
            _userRepository = userRespository;
        }

        public async Task<User> GetUserByAuthData(SignInUserDTO authData)
        {
            var user = await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));
            if(user == null) throw new ResultException(OperationErrorMessages.WrongAuthData);
            return user;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _userRepository.GetUserByLogin(login);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<User> InsertUser(SignUpUserDTO signUpUserDTO)
        {
            var newUser = Mapper.Map<User>(signUpUserDTO);

            var newUserId = await _userRepository.InsertUser(newUser);

            newUser.Id = newUserId;

            return newUser;
        }

        public AuthorizedUserDTO GetAuthorizedUser(User user)
        {
            var authorizedUser = Mapper.Map<AuthUser>(user);

            var identity = GetIdentity(authorizedUser);

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

            return new AuthorizedUserDTO
            {
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim("Login", user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
