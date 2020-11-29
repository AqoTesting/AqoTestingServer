using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes.CommonAPI
{
    public class CommonAPI_ValidateModelAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
                context.Result = ResultResponceExtension.ObjectResultResponse(OperationErrorMessages.InvalidModel, context.ModelState);

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
