using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        ICacheRepository _cache;
        public UserRepository(ICacheRepository cache)
        {
            _cache = cache;
        }
        public async Task<UsersDB_User_DTO> GetUserById(ObjectId userId) =>
            await _cache.Get<UsersDB_User_DTO>($"User:{userId}", () => UserWorker.GetUserById(userId));

        public async Task<UsersDB_User_DTO> GetUserByAuthData(string login, byte[] passwordHash) =>
            await Task.Run(() => UserWorker.GetUserByAuthData(login, passwordHash));

        public async Task<UsersDB_User_DTO> GetUserByLogin(string login) =>
            await Task.Run(() => UserWorker.GetUserByLogin(login));

        public async Task<UsersDB_User_DTO> GetUserByEmail(string email) =>
            await Task.Run(() => UserWorker.GetUserByEmail(email));

        public async Task<ObjectId> InsertUser(UsersDB_User_DTO user) =>
            await Task.Run(() => UserWorker.InsertUser(user));
    }
}