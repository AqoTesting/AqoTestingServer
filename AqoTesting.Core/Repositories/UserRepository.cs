using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        ICacheRepository _redisCache;
        public UserRepository(ICacheRepository cache)
        {
            _redisCache = cache;
        }
        public async Task<UsersDB_UserDTO> GetUserById(ObjectId userId) =>
            await _redisCache.Get($"User:{userId}", async () => await UserWorker.GetUserById(userId));

        public async Task<UsersDB_UserDTO> GetUserByAuthData(string login, byte[] passwordHash) =>
            await UserWorker.GetUserByAuthData(login, passwordHash);

        public async Task<UsersDB_UserDTO> GetUserByLogin(string login) =>
            await UserWorker.GetUserByLogin(login);

        public async Task<UsersDB_UserDTO> GetUserByEmail(string email) =>
            await UserWorker.GetUserByEmail(email);

        public async Task<ObjectId> InsertUser(UsersDB_UserDTO user) =>
            await UserWorker.InsertUser(user);

        public async Task<bool> SetProperty(ObjectId userId, string propertyName, object newPropertyValue)
        {
            await _redisCache.Del($"User:{userId}");

            return await UserWorker.SetProperty(userId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId userId, Dictionary<string, object> properties)
        {
            await _redisCache.Del($"User:{userId}");

            return await UserWorker.SetProperties(userId, properties);
        }
    }
}