using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
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
        ITokenGeneratorService _tokenGeneratorService;

        public UserService(IUserRepository userRespository, ITokenGeneratorService tokenGeneratorService)
        {
            _userRepository = userRespository;
            _tokenGeneratorService = tokenGeneratorService;
        }

        UsersDB_UserDTO user;

        public async Task<(OperationErrorMessages, object)> UserAPI_GetUserById(ObjectId userId) =>
            (user = await _userRepository.GetUserById(userId)) == null ?
                (OperationErrorMessages.UserNotFound, null) :

            (OperationErrorMessages.NoError,
            Mapper.Map<UserAPI_GetProfileDTO>(user) );

        public async Task<(OperationErrorMessages, object)> UserAPI_GetUserById(CommonAPI_UserIdDTO userIdDTO) =>
            await UserAPI_GetUserById(ObjectId.Parse(userIdDTO.UserId));

        public async Task<(OperationErrorMessages, object)> UserAPI_SignUp(UserAPI_SignUpDTO signUpDTO) =>
            await _userRepository.GetUserByLogin(signUpDTO.Login) != null ?
                (OperationErrorMessages.LoginAlreadyTaken, null) :

            await _userRepository.GetUserByEmail(signUpDTO.Email) != null ?
                (OperationErrorMessages.EmailAlreadyTaken, null) :

            (OperationErrorMessages.NoError,
            new CommonAPI_TokenDTO {
                Token = _tokenGeneratorService.GenerateToken(
                    await _userRepository.InsertUser(
                        Mapper.Map<UsersDB_UserDTO>(signUpDTO) ),
                    Role.User )});

        public async Task<(OperationErrorMessages, object)> UserAPI_SignIn(UserAPI_SignInDTO signInDTO) =>
            (user = await _userRepository.GetUserByAuthData(
                signInDTO.Login,
                Sha256.Compute(signInDTO.Password) )
            ) == null ?
                (OperationErrorMessages.WrongAuthData, null) :

            (OperationErrorMessages.NoError,
            new CommonAPI_TokenDTO { Token = _tokenGeneratorService.GenerateToken(
                user.Id,
                Role.User) });
    }
}