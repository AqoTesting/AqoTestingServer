using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByAuthData(SignInUserDTO authData);
        Task<User> GetUserByLogin(string login);
        Task<User> GetUserByEmail(string email);

        Task<User> InsertUser(SignUpUserDTO signUpUserDTO);

        AuthorizedUserDTO GetAuthorizedUser(User user);
    }
}
