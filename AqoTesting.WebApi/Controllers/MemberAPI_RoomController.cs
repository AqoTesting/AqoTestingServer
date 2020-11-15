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
            var (errorCode, response) = await _roomService.MemberAPI_GetRoomById(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [HttpGet("/member/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] RoomDomain_DTO roomDomainDTO)
        {
            var (errorCode, response) = await _roomService.MemberAPI_GetRoomByDomain(roomDomainDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}
