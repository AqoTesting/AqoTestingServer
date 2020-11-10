using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API;
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

        #region Itself
        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] RoomIdDTO roomIdDTO)
        {
            var room = await _roomService.GetUserRoomById(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] UserRoomDomainDTO roomDomainDTO)
        {
            var room = await _roomService.GetUserRoomByDomain(roomDomainDTO);

            if (room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/rooms")]
        public async Task<IActionResult> GetRoomsByOwnerId()
        {
            var rooms = await _roomService.GetUserRoomsByOwnerId(_workContext.UserId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/room/domainExists/{RoomDomain}")]
        public async Task<IActionResult> DomainExists([FromRoute] UserRoomDomainDTO roomDomainDTO)
        {
            var exists = await _roomService.GetUserRoomByDomain(roomDomainDTO);

            var result = exists != null;
            
            return this.ResultResponse(OperationErrorMessages.NoError, result);
        }


        [Auth(Role = Role.User)]
        [HttpPost("/user/room")]
        public async Task<IActionResult> CreateRoom([FromBody] PostUserRoomDTO newRoom)
        {
            var domainAlreadyTaken = await _roomService.GetUserRoomByDomain(newRoom.Domain);

            if (domainAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.DomainAlreadyTaken);

            var roomId = await _roomService.InsertRoom(newRoom);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPut("/user/room/{RoomId}")]
        public async Task<IActionResult> EditRoom([FromRoute] RoomIdDTO roomIdDTO, [FromBody] PostUserRoomDTO updatedRoom)
        {
            var errorCode = await _roomService.EditRoom(roomIdDTO, updatedRoom);

            return this.ResultResponse<object>(errorCode);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpDelete("/user/room/{RoomId}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] RoomIdDTO roomIdDTO)
        {
            await _roomService.DeleteRoomById(roomIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
        #endregion

        #region Members
        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}/members")]
        public async Task<IActionResult> GetMembersByRoomId([FromRoute] RoomIdDTO roomIdDTO)
        {
            var members = await _roomService.GetUserMembersByRoomId(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, members);
        }

        // Это надо переделать, всё завтра, хочу спать
        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpDelete("/user/room/{RoomId}/member/{MemberId}")]
        public async Task<IActionResult> KickMember([FromRoute] RoomIdDTO roomIdDTO, [FromRoute] MemberIdDTO memberTokenDTO)
        {
            await _roomService.RemoveMemberFromRoomByIdById(roomIdDTO, memberTokenDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
        #endregion
    }
}