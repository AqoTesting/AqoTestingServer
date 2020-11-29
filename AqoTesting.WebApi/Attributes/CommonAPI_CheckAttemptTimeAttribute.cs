using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using MongoDB.Bson;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CommonAPI_CheckAttemptTimeAttribute : ActionFilterAttribute
    {
        public CommonAPI_CheckAttemptTimeAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var workContext = context.HttpContext.RequestServices.GetService<IWorkContext>();
                var attemptRepository = context.HttpContext.RequestServices.GetService<IAttemptRepository>();

                var attemptsToCheck = new HashSet<AttemptsDB_Attempt_DTO>();

                if (workContext.MemberId != null)
                {
                    var attempt = await attemptRepository.GetActiveAttemptByMemberId(workContext.MemberId.Value);

                    if (attempt != null)
                        attemptsToCheck.Add(attempt);
                }

                foreach (var parameter in descriptor.MethodInfo.GetParameters())
                    if (parameter.ParameterType == typeof(CommonAPI_MemberId_DTO))
                    {
                        var attempt = await attemptRepository.GetActiveAttemptByMemberId(
                            ObjectId.Parse(((CommonAPI_MemberId_DTO)context.ActionArguments[parameter.Name]).MemberId));

                        if (attempt != null)
                            attemptsToCheck.Add(attempt);
                    }

                    else if (parameter.ParameterType == typeof(CommonAPI_AttemptId_DTO))
                    {
                        var attempt = await attemptRepository.GetAttemptById(
                            ObjectId.Parse(((CommonAPI_AttemptId_DTO)context.ActionArguments[parameter.Name]).AttemptId));

                        if (attempt != null)
                            attemptsToCheck.Add(attempt);
                    }

                    else if (parameter.ParameterType == typeof(CommonAPI_TestId_DTO))
                        (await attemptRepository.GetAttemptsByTestId(
                            ObjectId.Parse(((CommonAPI_TestId_DTO)context.ActionArguments[parameter.Name]).TestId)))
                                .Select(attempt =>
                                    attemptsToCheck.Add(attempt));

                if (attemptsToCheck.Count() > 0)
                {
                    var attemptService = context.HttpContext.RequestServices.GetService<IAttemptService>();

                    foreach (var attempt in attemptsToCheck)
                        if (attempt.StartDate != attempt.EndDate && DateTime.Now > attempt.EndDate)
                            await attemptService.CommonAPI_FinishAttempt(attempt);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
