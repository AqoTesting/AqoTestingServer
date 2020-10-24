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
            _workContext.UserId = ObjectId.Parse(context.Principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

            await base.TokenValidated(context);
        }
    }
}
