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
using AqoTesting.Shared.DTOs.API;

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
    }
}