using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserAPI_GetProfileDTO> GetUserById(ObjectId userId);
        Task<UserAPI_GetProfileDTO> GetUserById(CommonAPI_UserIdDTO userIdDTO);
        Task<UsersDB_UserDTO> GetUserByAuthData(UserAPI_SignInDTO authData);
        Task<UsersDB_UserDTO> GetUserByLogin(string login);
        Task<UsersDB_UserDTO> GetUserByEmail(string email);

        Task<UsersDB_UserDTO> InsertUser(UserAPI_SignUpDTO signUpUserDTO);
    }
}
