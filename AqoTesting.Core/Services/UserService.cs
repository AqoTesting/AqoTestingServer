using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using AutoMapper;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;

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
            if(user == null)
                throw new ResultException(OperationErrorMessages.UserNotFound);

            var responseUser = Mapper.Map<UserAPI_GetProfile_DTO>(user);

            return responseUser;
        }
        public async Task<UserAPI_GetProfile_DTO> GetUserById(CommonAPI_UserId_DTO userIdDTO) =>
            await GetUserById(ObjectId.Parse(userIdDTO.UserId));

        public async Task<UsersDB_User_DTO> GetUserByAuthData(UserAPI_SignIn_DTO authData)
        {
            var user = await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));

            return user;
        }

        public async Task<UsersDB_User_DTO> GetUserByLogin(string login) =>
            await _userRepository.GetUserByLogin(login);

        public async Task<UsersDB_User_DTO> GetUserByEmail(string email) =>
            await _userRepository.GetUserByEmail(email);

        public async Task<UsersDB_User_DTO> InsertUser(UserAPI_SignUp_DTO signUpUserDTO)
        {
            var newUser = Mapper.Map<UsersDB_User_DTO>(signUpUserDTO);

            var newUserId = await _userRepository.InsertUser(newUser);

            newUser.Id = newUserId;

            return newUser;
        }
    }
}