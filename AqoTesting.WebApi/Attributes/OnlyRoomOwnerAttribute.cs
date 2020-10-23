using AqoTesting.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;

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

            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    if (parameter.Name != "roomId") continue;

                    if (!context.ActionArguments.ContainsKey(parameter.Name))
                    {
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(OperationErrorMessages.EntityNotFound);
                        break;
                    };

                    var argument = context.ActionArguments[parameter.Name];

                    var evalResult = EvaluateValidationAttributes(argument, context.HttpContext, _roomService);
                    if (evalResult != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(evalResult);
                }
            }
            
            base.OnActionExecuting(context);
        }
        private OperationErrorMessages EvaluateValidationAttributes(object argument, HttpContext httpContext, IRoomService roomService)
        {
            ObjectId roomId = ObjectId.Parse(argument.ToString());
            ObjectId ownerId = ObjectId.Parse(httpContext.User.FindFirst("Id").Value);
            Room room = roomService.GetRoomById(roomId).Result;

            if (room == null) {
                return OperationErrorMessages.RoomDoesntExist;
            } 
            else if (room.OwnerId != ownerId)
            {
                return OperationErrorMessages.RoomAccessError;
            }

            return OperationErrorMessages.NoError;
        }
    }
}
