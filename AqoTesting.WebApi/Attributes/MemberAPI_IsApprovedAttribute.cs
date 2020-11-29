using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    public class MemberAPI_IsApprovedAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();
            var _memberService = context.HttpContext.RequestServices.GetService<IMemberService>();
            var (result, member) = await _memberService.MemberAPI_GetMemberById(_workContext.MemberId.Value);

            if(result == OperationErrorMessages.NoError && !(member as MemberAPI_GetProfile_DTO).IsApproved)
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberIsNotApproved);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
