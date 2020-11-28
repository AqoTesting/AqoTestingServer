using System.Collections.Generic;
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

        Task<bool> SetProperty(ObjectId userId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId userId, Dictionary<string, object> properties);
    }
}
