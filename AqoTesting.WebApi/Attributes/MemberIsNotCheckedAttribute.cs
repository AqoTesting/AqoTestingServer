using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    public class MemberIsNotChecked : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();

            if(_workContext.IsChecked)
            {
                context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.MemberIsChecked);
            }

            base.OnActionExecuting(context);
        }
    }
}
