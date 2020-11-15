using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AqoTesting.WebApi.Attributes
{
    public class MemberIsCheckedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();

            if(!_workContext.IsChecked)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberNotIsChecked);
            }

            base.OnActionExecuting(context);
        }
    }
}
