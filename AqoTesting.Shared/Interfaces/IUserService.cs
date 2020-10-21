using AqoTesting.Shared.DTOs.API.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IUserService
    {
        Task<AuthorizedUserDTO> SignIn(string login, string password);
    }
}
