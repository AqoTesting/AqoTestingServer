using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.BD.Users;
using AqoTesting.Shared.Interfaces;

namespace AqoTesting.Core.Repositories
{
    public class UserRespository : IUserRespository {
        public async Task<User> GetUserByAuthData(string login, byte[] passwordHash) {
            return MongoIOController.GetUserByAuthData(login, passwordHash);
        }
    }
}
