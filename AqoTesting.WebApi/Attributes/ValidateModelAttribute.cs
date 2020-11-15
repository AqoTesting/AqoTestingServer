using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AqoTesting.WebApi.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<ModelStateDictionary>(OperationErrorMessages.InvalidModel, context.ModelState);
            }
            base.OnActionExecuting(context);
        }
    }
}
