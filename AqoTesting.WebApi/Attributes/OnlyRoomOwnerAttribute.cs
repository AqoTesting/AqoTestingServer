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
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OnlyRoomOwnerAttribute : ActionFilterAttribute
    {
        public OnlyRoomOwnerAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var _roomService = context.HttpContext.RequestServices.GetService<IRoomService>();
            var _roomRepository = context.HttpContext.RequestServices.GetService<IRoomRepository>();

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

                    var evalResult = await EvaluateValidationAttributes(argument, context.HttpContext, _roomRepository, parameter.Name);
                    if(evalResult != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(evalResult);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<OperationErrorMessages> EvaluateValidationAttributes(object argument, HttpContext httpContext, IRoomRepository roomRepository, string dtoName)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var ownerId = _workContext.UserId;

            var room = new RoomsDB_Room_DTO();

            if(dtoName == "roomIdDTO")
            {
                room = await roomRepository.GetRoomById(ObjectId.Parse(((CommonAPI_RoomId_DTO)argument).RoomId));
            }
            else if(dtoName == "roomDomainDTO")
            {
                room = await roomRepository.GetRoomByDomain(((CommonAPI_RoomDomain_DTO)argument).RoomDomain);
            }

            if(room == null)
                return OperationErrorMessages.RoomNotFound;

            else if(room.OwnerId != ownerId)
                return OperationErrorMessages.RoomAccessError;

            return OperationErrorMessages.NoError;
        }
    }
}
