using AqoTesting.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

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
