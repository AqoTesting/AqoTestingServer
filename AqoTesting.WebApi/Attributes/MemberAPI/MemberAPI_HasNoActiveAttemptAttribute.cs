using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes.MemberAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MemberAPI_HasNoActiveAttemptAttribute : ActionFilterAttribute
    {
        public MemberAPI_HasNoActiveAttemptAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _attemptRepository = context.HttpContext.RequestServices.GetService<IAttemptRepository>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var errorCode = await EvaluateValidationAttributes(context.HttpContext, _attemptRepository);
                if(errorCode != OperationErrorMessages.NoError)
                    context.Result = ResultResponceExtension.ObjectResultResponse<object>(errorCode);
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<OperationErrorMessages> EvaluateValidationAttributes(HttpContext httpContext, IAttemptRepository attemptRepository)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();
            var memberId = _workContext.MemberId.Value;

            var activeAttempt = await attemptRepository.GetActiveAttemptByMemberId(memberId);
            if(activeAttempt != null)
                return OperationErrorMessages.HasActiveAttempt;

            return OperationErrorMessages.NoError;
        }
    }
}
