using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;

namespace AqoTesting.WebApi.Attributes
{
    public class MemberIsApprovedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();
            var _memberService = context.HttpContext.RequestServices.GetService<IMemberService>();
            var (result, member) = _memberService.MemberAPI_GetMemberById(_workContext.MemberId).Result;

            if(result == OperationErrorMessages.NoError && !(member as MemberAPI_GetProfile_DTO).IsApproved)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberIsNotApproved);
            }

            base.OnActionExecuting(context);
        }
    }
}
