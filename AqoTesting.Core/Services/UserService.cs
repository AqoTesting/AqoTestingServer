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
        IUserRepository _userRepository;

        public UserService(IUserRepository userRespository, IWorkContext workContext)
        {
            _userRepository = userRespository;
        }

        public async Task<GetUserDTO> GetUserById(ObjectId userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null) throw new ResultException(OperationErrorMessages.UserNotFound);

            var responseUser = Mapper.Map<GetUserDTO>(user);

            return responseUser;
        }
        public async Task<GetUserDTO> GetUserById(UserIdDTO userIdDTO) =>
            await GetUserById(ObjectId.Parse(userIdDTO.UserId));

        public async Task<User> GetUserByAuthData(SignInUserDTO authData)
        {
            var user = await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));
            if (user == null) throw new ResultException(OperationErrorMessages.WrongAuthData);
            return user;
        }

        public async Task<User> GetUserByLogin(string login) =>
            await _userRepository.GetUserByLogin(login);

        public async Task<User> GetUserByEmail(string email) =>
            await _userRepository.GetUserByEmail(email);

        public async Task<User> InsertUser(SignUpUserDTO signUpUserDTO)
        {
            var newUser = Mapper.Map<User>(signUpUserDTO);

            var newUserId = await _userRepository.InsertUser(newUser);

            newUser.Id = newUserId;

            return newUser;
        }

        public GetTokenDTO GenerateToken(ObjectId id, Role role = Role.User)
        {
            var identity = GetIdentity(id, role);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new GetTokenDTO { Token = encodedJwt };
        }

        private ClaimsIdentity GetIdentity(ObjectId id, Role role)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}