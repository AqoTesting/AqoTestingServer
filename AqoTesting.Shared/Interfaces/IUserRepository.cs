using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserRepository
    {
        Task<UsersDB_User_DTO> GetUserById(ObjectId userId);
        Task<UsersDB_User_DTO> GetUserByAuthData(string login, byte[] passwordHash);
        Task<UsersDB_User_DTO> GetUserByLogin(string login);
        Task<UsersDB_User_DTO> GetUserByEmail(string email);
        Task<ObjectId> InsertUser(UsersDB_User_DTO user);
    }
}
