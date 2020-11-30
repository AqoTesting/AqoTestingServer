using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_RoomController : Controller
    {

        IRoomService _roomService;
        IWorkContext _workContext;

        public MemberAPI_RoomController(IRoomService roomService, IWorkContext workContext)
        {
            _roomService = roomService;
            _workContext = workContext;
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [HttpGet("/member/room")]
        public async Task<IActionResult> GetRoom()
        {
            var (errorCode, response) = await _roomService.MemberAPI_GetRoomById(_workContext.RoomId.Value);

            return this.ResultResponse(errorCode, response);
        }

        [HttpGet("/member/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] CommonAPI_RoomIdDTO roomIdDTO)
        {
            var (errorCode, response) = await _roomService.MemberAPI_GetRoomById(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [HttpGet("/member/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] CommonAPI_RoomDomainDTO roomDomainDTO)
        {
            var (errorCode, response) = await _roomService.MemberAPI_GetRoomByDomain(roomDomainDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}
