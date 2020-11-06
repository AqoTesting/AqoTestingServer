using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}")]
        public async Task<IActionResult> GetRoom([FromRoute] RoomIdDTO roomIdDTO)
        {
            var room = await _roomService.GetRoomById(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Authorize(Roles = "User")]
        [HttpGet("/user/rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _roomService.GetRoomsByOwnerId(_workContext.UserId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Authorize(Roles = "User")]
        [HttpPost("/user/room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO newRoom)
        {
            var domainAlreadyTaken = await _roomService.GetRoomByDomain(newRoom.Domain);

            if (domainAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.DomainAlreadyTaken);

            var roomId = await _roomService.InsertRoom(newRoom);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpPatch("/user/room/{RoomId}")]
        public async Task<IActionResult> EditRoom([FromRoute] RoomIdDTO roomIdDTO, [FromBody] EditRoomDTO roomUpdates)
        {
            var updatedRoom = await _roomService.EditRoom(roomIdDTO, roomUpdates);

            return this.ResultResponse<object>(OperationErrorMessages.NoError, updatedRoom);
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}/edit")]
        public async Task<IActionResult> GetEditRoom([FromRoute] RoomIdDTO roomIdDTO)
        {
            var room = await _roomService.GetEditRoomById(roomIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError, room);
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpDelete("/user/room/{RoomId}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] RoomIdDTO roomIdDTO)
        {
            await _roomService.DeleteRoomById(roomIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpDelete("/user/room/{RoomId}/member/{MemberId}")]
        public async Task<IActionResult> KickMember([FromRoute] RoomIdDTO roomIdDTO, [FromRoute] MemberIdDTO memberTokenDTO)
        {
            await _roomService.RemoveMemberFromRoomByIdById(roomIdDTO, memberTokenDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
    }
}