﻿using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;

namespace AqoTesting.WebApi.Attributes.MemberAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MemberAPI_AttemptAccessAttribute : ActionFilterAttribute
    {
        public MemberAPI_AttemptAccessAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _attemptRepository = context.HttpContext.RequestServices.GetService<IAttemptRepository>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach(var parameter in parameters)
                {
                    if(parameter.ParameterType != typeof(CommonAPI_AttemptId_DTO))
                        continue;

                    var argument = context.ActionArguments[parameter.Name];

                    var (valid, errorCode) = await EvaluateValidationAttributes(argument, context.HttpContext, _attemptRepository, parameter.ParameterType);
                    if(!valid)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(errorCode);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<(bool, OperationErrorMessages)> EvaluateValidationAttributes(object argument, HttpContext httpContext, IAttemptRepository attemptRepository, Type dtoType)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var memberId = _workContext.MemberId.Value;
            var attempt = await attemptRepository.GetAttemptById(ObjectId.Parse(((CommonAPI_AttemptId_DTO)argument).AttemptId));

            if(attempt == null)
                return (false, OperationErrorMessages.AttemptNotFound);

            else if(attempt.MemberId != memberId)
                return (false, OperationErrorMessages.AttemptAccessError);

            return (true, OperationErrorMessages.NoError);
        }
    }
}
