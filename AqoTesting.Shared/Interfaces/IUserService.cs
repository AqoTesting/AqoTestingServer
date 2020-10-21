using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.BD.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByAuthData(SignInUserDTO authData);
        Task<string> GenerateJwtToken(User user);
    }
}
