using AqoTesting.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    public class AuthAttribute : AuthorizeAttribute
    {
        private Role role;
        public Role Role
        {
            get { return role; }
            set { role = value; base.Roles = value.ToString(); }
        }
    }
}
