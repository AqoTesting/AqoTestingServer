using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using System;
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
            Enum.TryParse(context.Principal.Claims.Single(c => c.Type == ClaimTypes.Role).Value, out Role role);

            ObjectId id = ObjectId.Parse(context.Principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (role == Role.User)
            {
                _workContext.UserId = id;
            } else if(role == Role.Member)
            {
                _workContext.MemberId = id;
                _workContext.IsChecked = bool.Parse(context.Principal.Claims.Single(c => c.Type == "isChecked").Value);
                _workContext.RoomId = ObjectId.Parse(context.Principal.Claims.Single(c => c.Type == "roomId").Value);
            } else
            {
                context.Fail("Authentication failed");
            }

            await base.TokenValidated(context);
        }
    }
}
