using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<ModelStateDictionary>(OperationErrorMessages.InvalidModel, context.ModelState);
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
