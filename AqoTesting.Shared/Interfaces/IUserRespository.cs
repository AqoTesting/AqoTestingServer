using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserRespository
    {
        Task<User> GetUserByAuthData(string login, byte[] passwordHash);
        Task<User> GetUserByLogin(string login);
        Task<User> GetUserByEmail(string email);
        Task<ObjectId> InsertUser(User user);
    }
}
