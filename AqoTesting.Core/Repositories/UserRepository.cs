using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUserByAuthData(string login, byte[] passwordHash)
        {
            return await Task.Run(() => UserWorker.GetUserByAuthData(login, passwordHash));
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await Task.Run(() => UserWorker.GetUserByLogin(login));
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await Task.Run(() => UserWorker.GetUserByEmail(email));
        }

        public async Task<ObjectId> InsertUser(User user)
        {
            return await Task.Run(() => UserWorker.InsertUser(user));
        }
    }
}
