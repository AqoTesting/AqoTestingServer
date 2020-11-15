using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserAPI_RoomController : Controller
    {
        IRoomService _roomService;
        IWorkContext _workContext;

        public UserAPI_RoomController(IRoomService roomService, IWorkContext workContext)
        {
            _roomService = roomService;
            _workContext = workContext;
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] RoomId_DTO roomIdDTO)
        {
            var room = await _roomService.UserAPI_GetRoomById(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] UserAPI_RoomDomain_DTO roomDomainDTO)
        {
            var room = await _roomService.UserAPI_GetRoomByDomain(roomDomainDTO);

            if(room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/rooms")]
        public async Task<IActionResult> GetRoomsByOwnerId()
        {
            var rooms = await _roomService.UserAPI_GetRoomsByOwnerId(_workContext.UserId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/room/domainExists/{RoomDomain}")]
        public async Task<IActionResult> DomainExists([FromRoute] UserAPI_RoomDomain_DTO roomDomainDTO)
        {
            var exists = await _roomService.UserAPI_GetRoomByDomain(roomDomainDTO);

            var result = exists != null;

            return this.ResultResponse(OperationErrorMessages.NoError, result);
        }


        [Auth(Role = Role.User)]
        [HttpPost("/user/room")]
        public async Task<IActionResult> CreateRoom([FromBody] UserAPI_PostRoom_DTO newRoom)
        {
            var domainAlreadyTaken = await _roomService.UserAPI_GetRoomByDomain(newRoom.Domain);

            if(domainAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.DomainAlreadyTaken);

            var roomId = await _roomService.UserAPI_InsertRoom(newRoom);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPut("/user/room/{RoomId}")]
        public async Task<IActionResult> EditRoom([FromRoute] RoomId_DTO roomIdDTO, [FromBody] UserAPI_PostRoom_DTO updatedRoom)
        {
            var errorCode = await _roomService.UserAPI_EditRoom(roomIdDTO, updatedRoom);

            return this.ResultResponse<object>(errorCode);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpDelete("/user/room/{RoomId}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] RoomId_DTO roomIdDTO)
        {
            await _roomService.UserAPI_DeleteRoomById(roomIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
    }
}