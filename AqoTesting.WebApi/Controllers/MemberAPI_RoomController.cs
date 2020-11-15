using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_RoomController : Controller
    {

        IRoomService _roomService;

        public MemberAPI_RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("/member/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] RoomId_DTO roomIdDTO)
        {
            var room = await _roomService.MemberAPI_GetRoomById(roomIdDTO);

            if(room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [HttpGet("/member/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] MemberAPI_RoomDomain_DTO roomDomainDTO)
        {
            var room = await _roomService.MemberAPI_GetRoomByDomain(roomDomainDTO);

            if(room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }
    }
}
