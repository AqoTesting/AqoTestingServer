using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserRepository
    {
        Task<UsersDB_UserDTO> GetUserById(ObjectId userId);

        Task<UsersDB_UserDTO> GetUserByAuthData(string login, byte[] passwordHash);

        Task<UsersDB_UserDTO> GetUserByLogin(string login);
        Task<UsersDB_UserDTO> GetUserByEmail(string email);

        Task<ObjectId> InsertUser(UsersDB_UserDTO user);

        Task<bool> SetProperty(ObjectId userId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId userId, Dictionary<string, object> properties);
    }
}
