using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;

namespace AqoTesting.WebApi.Attributes
{
    public class MemberIsNotApprovedAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();
            var _memberService = context.HttpContext.RequestServices.GetService<IMemberService>();
            var (result, member) =  await _memberService.MemberAPI_GetMemberById(_workContext.MemberId);

            if (result == OperationErrorMessages.NoError && (member as MemberAPI_GetProfile_DTO).IsApproved)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberIsApproved);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
