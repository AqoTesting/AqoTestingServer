using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserAPI_GetProfile_DTO> GetUserById(ObjectId userId);
        Task<UserAPI_GetProfile_DTO> GetUserById(UserId_DTO userIdDTO);
        Task<User> GetUserByAuthData(UserAPI_SignIn_DTO authData);
        Task<User> GetUserByLogin(string login);
        Task<User> GetUserByEmail(string email);

        Task<User> InsertUser(UserAPI_SignUp_DTO signUpUserDTO);
    }
}
