using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class UserRespository : IUserRespository {
        public async Task<User> GetUserByAuthData(string login, byte[] passwordHash) {
            return await Task.Run(() => MongoIOController.GetUserByAuthData(login, passwordHash));
        }

        public async Task<User> GetUserByLogin(string login) {
            return await Task.Run(() => MongoIOController.GetUserByLogin(login));
        }

        public async Task<User> GetUserByEmail(string email) {
            return await Task.Run(() => MongoIOController.GetUserByEmail(email));
        }

        public async Task<ObjectId> InsertUser(User user) {
            return await Task.Run(() => MongoIOController.InsertUser(user));
        }
    }
}
