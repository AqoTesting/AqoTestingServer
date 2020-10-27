using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<ModelStateDictionary>(OperationErrorMessages.InvalidModel, context.ModelState);
            }
            base.OnActionExecuting(context);
        }
    }
}
