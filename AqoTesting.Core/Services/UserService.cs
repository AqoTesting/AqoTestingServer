using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using AutoMapper;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.API.Common;

namespace AqoTesting.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        IUserRepository _userRepository;

        public UserService(IUserRepository userRespository)
        {
            _userRepository = userRespository;
        }

        public async Task<UserAPI_GetProfile_DTO> GetUserById(ObjectId userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if(user == null) throw new ResultException(OperationErrorMessages.UserNotFound);

            var responseUser = Mapper.Map<UserAPI_GetProfile_DTO>(user);

            return responseUser;
        }
        public async Task<UserAPI_GetProfile_DTO> GetUserById(UserId_DTO userIdDTO) =>
            await GetUserById(ObjectId.Parse(userIdDTO.UserId));

        public async Task<User> GetUserByAuthData(UserAPI_SignIn_DTO authData)
        {
            var user = await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));

            return user;
        }

        public async Task<User> GetUserByLogin(string login) =>
            await _userRepository.GetUserByLogin(login);

        public async Task<User> GetUserByEmail(string email) =>
            await _userRepository.GetUserByEmail(email);

        public async Task<User> InsertUser(UserAPI_SignUp_DTO signUpUserDTO)
        {
            var newUser = Mapper.Map<User>(signUpUserDTO);

            var newUserId = await _userRepository.InsertUser(newUser);

            newUser.Id = newUserId;

            return newUser;
        }
    }
}