using AqoTesting.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AqoTesting.WebApi.Attributes.CommonAPI
{
    public class CommonAPI_AuthAttribute : AuthorizeAttribute
    {
        private Role role;
        public Role Role
        {
            get { return role; }
            set { role = value; Roles = value.ToString(); }
        }
    }
}
