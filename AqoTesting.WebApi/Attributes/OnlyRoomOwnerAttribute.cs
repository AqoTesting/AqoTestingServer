using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.API.Common;

namespace AqoTesting.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OnlyRoomOwnerAttribute : ActionFilterAttribute
    {
        public OnlyRoomOwnerAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var _roomService = context.HttpContext.RequestServices.GetService<IRoomService>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach(var parameter in parameters)
                {
                    if(parameter.Name != "roomIdDTO" && parameter.Name != "roomDomainDTO") continue;

                    if(!context.ActionArguments.ContainsKey(parameter.Name))
                    {
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.EntityNotFound);
                        break;
                    };

                    var argument = context.ActionArguments[parameter.Name];

                    var evalResult = EvaluateValidationAttributes(argument, context.HttpContext, _roomService, parameter.Name);
                    if(evalResult != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(evalResult);
                }
            }

            base.OnActionExecuting(context);
        }
        private OperationErrorMessages EvaluateValidationAttributes(object argument, HttpContext httpContext, IRoomService roomService, string dtoName)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var ownerId = _workContext.UserId.ToString();
            UserAPI_GetRoom_DTO room = null;

            if(dtoName == "roomIdDTO")
            {
                room = roomService.UserAPI_GetRoomById((RoomId_DTO)argument).Result;
            }
            else if(dtoName == "roomDomainDTO")
            {
                room = roomService.UserAPI_GetRoomByDomain((UserAPI_RoomDomain_DTO)argument).Result;
            }

            if(room == null)
                return OperationErrorMessages.RoomNotFound;

            else if(room.OwnerId != ownerId)
                return OperationErrorMessages.RoomAccessError;

            return OperationErrorMessages.NoError;
        }
    }
}
