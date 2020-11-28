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
using AqoTesting.Shared.DTOs.API.Common.Identifiers;

namespace AqoTesting.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserAPI_MemberAccessAttribute : ActionFilterAttribute
    {
        public UserAPI_MemberAccessAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _memberRepository = context.HttpContext.RequestServices.GetService<IMemberRepository>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType != typeof(CommonAPI_MemberId_DTO))
                        continue;

                    var argument = context.ActionArguments[parameter.Name];

                    var errorCode = await EvaluateValidationAttributes(argument, context.HttpContext, _memberRepository, parameter.ParameterType);
                    if (errorCode != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(errorCode);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<OperationErrorMessages> EvaluateValidationAttributes(object argument, HttpContext httpContext, IMemberRepository memberRepository, Type dtoType)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var userId = _workContext.UserId;
            var member = await memberRepository.GetMemberById(ObjectId.Parse(((CommonAPI_MemberId_DTO) argument).MemberId));

            if (member == null)
                return OperationErrorMessages.MemberNotFound;
            else if (member.UserId != userId)
                return OperationErrorMessages.MemberAccessError;

            return OperationErrorMessages.NoError;
        }
    }
}
