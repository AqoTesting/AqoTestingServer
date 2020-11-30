using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.UserAPI;
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

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpGet("/user/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] CommonAPI_RoomIdDTO roomIdDTO)
        {
            var (errorCode, response) = await _roomService.UserAPI_GetRoomById(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpGet("/user/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] CommonAPI_RoomDomainDTO roomDomainDTO)
        {
            var (errorCode, response) = await _roomService.UserAPI_GetRoomByDomain(roomDomainDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [HttpGet("/user/rooms")]
        public async Task<IActionResult> GetRoomsByUserId()
        {
            var (errorCode, response) = await _roomService.UserAPI_GetRoomsByUserId(_workContext.UserId.Value);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [HttpGet("/user/room/domainExists/{RoomDomain}")]
        public async Task<IActionResult> DomainExists([FromRoute] CommonAPI_RoomDomainDTO roomDomainDTO)
        {
            var (errorCode, response) = await _roomService.UserAPI_GetRoomByDomain(roomDomainDTO);

            return this.ResultResponse(errorCode, response);
        }


        [CommonAPI_Auth(Role = Role.User)]
        [HttpPost("/user/room")]
        public async Task<IActionResult> CreateRoom([FromBody] UserAPI_PostRoomDTO newRoom)
        {
            var (errorCode, response) = await _roomService.UserAPI_CreateRoom(newRoom);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpPut("/user/room/{RoomId}")]
        public async Task<IActionResult> EditRoom([FromRoute] CommonAPI_RoomIdDTO roomIdDTO, [FromBody] UserAPI_PostRoomDTO updatedRoom)
        {
            var errorCode = await _roomService.UserAPI_EditRoom(roomIdDTO, updatedRoom);

            return this.ResultResponse<object>(errorCode);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpDelete("/user/room/{RoomId}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] CommonAPI_RoomIdDTO roomIdDTO)
        {
            var errorCode = await _roomService.UserAPI_DeleteRoomById(roomIdDTO);

            return this.ResultResponse<object>(errorCode);
        }
    }
}