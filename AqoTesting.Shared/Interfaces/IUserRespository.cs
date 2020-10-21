using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.BD.Users;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserRespository
    {
        Task<User> GetUserByAuthData(string login, byte[] passwordHash);
    }
}
