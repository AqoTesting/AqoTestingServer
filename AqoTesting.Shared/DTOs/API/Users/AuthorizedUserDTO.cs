using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.API.Users
{
    class AuthorizedUserDTO
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
