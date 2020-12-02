using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AqoTesting.Shared.Infrastructure;
using System.Linq;

namespace AqoTesting.WebApi.Attributes.UserAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class InternalAPI_AccessAttribute : ActionFilterAttribute
    {
        public InternalAPI_AccessAttribute() { }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach(var parameter in parameters)
                {
                    if(parameter.ParameterType.GetInterfaces().Contains(typeof(IInternalAPI_PostDTO)))
                    {
                        var config = context.HttpContext.RequestServices.GetService<IOptions<InternalApiConfig>>();

                        var requestAccessKey = ((IInternalAPI_PostDTO)context.ActionArguments[parameter.Name]).AccessKey;
                        var correctAccessKey = config.Value.AccessKey;
                        if(requestAccessKey != correctAccessKey)
                        {
                            context.Result = ResultResponceExtension.ObjectResultResponse<object>(
                                OperationErrorMessages.InternalApiWrongAccessKey);

                            break;
                        }
                    }
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
