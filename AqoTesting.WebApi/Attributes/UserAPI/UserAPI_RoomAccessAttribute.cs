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
using AqoTesting.Shared.DTOs.DB.Rooms;

namespace AqoTesting.WebApi.Attributes.UserAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserAPI_RoomAccessAttribute : ActionFilterAttribute
    {
        public UserAPI_RoomAccessAttribute()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _roomRepository = context.HttpContext.RequestServices.GetService<IRoomRepository>();

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if(descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach(var parameter in parameters)
                {
                    if(parameter.ParameterType != typeof(CommonAPI_RoomIdDTO) && parameter.ParameterType != typeof(CommonAPI_RoomDomainDTO))
                        continue;

                    var argument = context.ActionArguments[parameter.Name];

                    var errorCode = await EvaluateValidationAttributes(argument, context.HttpContext, _roomRepository, parameter.ParameterType);
                    if(errorCode != OperationErrorMessages.NoError)
                        context.Result = ResultResponceExtension.ObjectResultResponse<object>(errorCode);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        private async Task<OperationErrorMessages> EvaluateValidationAttributes(object argument, HttpContext httpContext, IRoomRepository roomRepository, Type dtoType)
        {
            var _workContext = httpContext.RequestServices.GetService<IWorkContext>();

            var userId = _workContext.UserId.Value;
            var room = new RoomsDB_RoomDTO();

            if(dtoType == typeof(CommonAPI_RoomIdDTO))
                room = await roomRepository.GetRoomById(ObjectId.Parse(((CommonAPI_RoomIdDTO)argument).RoomId));
            else if(dtoType == typeof(CommonAPI_RoomDomainDTO))
                room = await roomRepository.GetRoomByDomain(((CommonAPI_RoomDomainDTO)argument).RoomDomain);

            if(room == null)
                return OperationErrorMessages.RoomNotFound;

            else if(room.UserId != userId)
                return OperationErrorMessages.RoomAccessError;

            return OperationErrorMessages.NoError;
        }
    }
}
