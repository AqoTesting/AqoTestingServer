using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetUserById(ObjectId userId);
        Task<(OperationErrorMessages, object)> UserAPI_GetUserById(CommonAPI_UserIdDTO userIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_CheckRegisteredByLogin(string login);
        Task<(OperationErrorMessages, object)> UserAPI_CheckRegisteredByEmail(string email);

        Task<(OperationErrorMessages, object)> UserAPI_SignUp(UserAPI_SignUpDTO signUpDTO);
        Task<(OperationErrorMessages, object)> UserAPI_SignIn(UserAPI_SignInDTO signInDTO);
    }
}
