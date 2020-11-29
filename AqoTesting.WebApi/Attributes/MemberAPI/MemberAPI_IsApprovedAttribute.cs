using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes.MemberAPI
{
    public class MemberAPI_IsApprovedAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();
            var _memberRepository = context.HttpContext.RequestServices.GetService<IMemberRepository>();
            var member = await _memberRepository.GetMemberById(_workContext.MemberId.Value);

            if(!member.IsApproved)
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberIsNotApproved);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
