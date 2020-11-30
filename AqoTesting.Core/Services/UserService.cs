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

        public async Task<UserAPI_GetProfileDTO> GetUserById(ObjectId userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if(user == null)
                throw new ResultException(OperationErrorMessages.UserNotFound);

            var responseUser = Mapper.Map<UserAPI_GetProfileDTO>(user);

            return responseUser;
        }
        public async Task<UserAPI_GetProfileDTO> GetUserById(CommonAPI_UserIdDTO userIdDTO) =>
            await GetUserById(ObjectId.Parse(userIdDTO.UserId));

        public async Task<UsersDB_UserDTO> GetUserByAuthData(UserAPI_SignInDTO authData)
        {
            var user = await _userRepository.GetUserByAuthData(authData.Login, Sha256.Compute(authData.Password));

            return user;
        }

        public async Task<UsersDB_UserDTO> GetUserByLogin(string login) =>
            await _userRepository.GetUserByLogin(login);

        public async Task<UsersDB_UserDTO> GetUserByEmail(string email) =>
            await _userRepository.GetUserByEmail(email);

        public async Task<UsersDB_UserDTO> InsertUser(UserAPI_SignUpDTO signUpUserDTO)
        {
            var newUser = Mapper.Map<UsersDB_UserDTO>(signUpUserDTO);

            var newUserId = await _userRepository.InsertUser(newUser);

            newUser.Id = newUserId;

            return newUser;
        }
    }
}