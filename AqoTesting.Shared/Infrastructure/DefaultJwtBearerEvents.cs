using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            //context.Principal.
        }
    }
}
