using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserAPI_GetProfile_DTO> GetUserById(ObjectId userId);
        Task<UserAPI_GetProfile_DTO> GetUserById(CommonAPI_UserId_DTO userIdDTO);
        Task<UsersDB_User_DTO> GetUserByAuthData(UserAPI_SignIn_DTO authData);
        Task<UsersDB_User_DTO> GetUserByLogin(string login);
        Task<UsersDB_User_DTO> GetUserByEmail(string email);

        Task<UsersDB_User_DTO> InsertUser(UserAPI_SignUp_DTO signUpUserDTO);
    }
}
