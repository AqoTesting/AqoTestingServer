using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserAPI_TestAccessAttribute : ActionFilterAttribute
    {
        public UserAPI_TestAccessAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _testRepository = context.HttpContext.RequestServices.GetService<ITestRepository>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType != typeof(CommonAPI_TestId_DTO))
                        continue;

                    var argument = context.ActionArguments[parameter.Name];

                    var errorCode = await EvaluateValidationAttributes(argument, context.HttpContext, _testRepository, parameter.ParameterType);
                    if (errorCode != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(errorCode);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<OperationErrorMessages> EvaluateValidationAttributes(object argument, HttpContext httpContext, ITestRepository testRepository, Type dtoType)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var userId = _workContext.UserId;
            var test = await testRepository.GetTestById(ObjectId.Parse(((CommonAPI_TestId_DTO) argument).TestId));

            if (test == null)
                return OperationErrorMessages.TestNotFound;
            else if (test.UserId != userId)
                return OperationErrorMessages.TestAccessError;

            return OperationErrorMessages.NoError;
        }
    }
}
