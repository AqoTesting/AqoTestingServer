using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Infrastructure
{
    public class DefaultJwtBearerEvents : JwtBearerEvents
    {
        private readonly IWorkContext _workContext;
        public DefaultJwtBearerEvents(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            string role = context.Principal.Claims.Single(c => c.Type == ClaimTypes.Role).Value;
            ObjectId id = ObjectId.Parse(context.Principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            bool isChecked = bool.Parse(context.Principal.Claims.Single(c => c.Type == "isChecked").Value);

            if (role == "User")
            {
                _workContext.UserId = id;
            } else
            {
                _workContext.MemberId = id;
                _workContext.IsChecked = isChecked;
            }

            await base.TokenValidated(context);
        }
    }
}
