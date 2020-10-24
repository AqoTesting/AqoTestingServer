using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserRoomController : Controller
    {
        IWorkContext _workContext;
        IRoomService _roomService;
            
        public UserRoomController(IWorkContext workContext, IRoomService roomService)
        {
            _workContext = workContext;
            _roomService = roomService;
        }

        [Authorize]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{Id}")]
        public async Task<IActionResult> GetRoom([FromRoute] RoomIdDTO roomIdDTO)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            var room = await _roomService.GetRoomById(roomIdDTO.Id);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Authorize]
        [HttpGet("/user/rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _roomService.GetRoomsByOwnerId(_workContext.UserId.ToString());

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Authorize]
        [HttpPost("/user/room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO newRoom)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            var domainAlreadyTaken = await _roomService.GetRoomByDomain(newRoom.Domain);

            if (domainAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.DomainAlreadyTaken);

            var roomId = await _roomService.InsertRoom(newRoom);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }

        [Authorize]
        [OnlyRoomOwner]
        [HttpDelete("/user/room")]
        public async Task<IActionResult> DeleteRoom([FromBody] RoomIdDTO oldRoom)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            var deleted = await _roomService.DeleteRoomById(oldRoom.Id);

            if (deleted)
                return this.ResultResponse<object>(OperationErrorMessages.NoError);

            else
                throw new ResultException(OperationErrorMessages.RoomDoesntExists);
        }
    }
}