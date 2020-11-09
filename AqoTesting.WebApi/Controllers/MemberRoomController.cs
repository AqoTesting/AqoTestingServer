using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberRoomController : Controller
    {

        IRoomService _roomService;

        public MemberRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("/member/room/{RoomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] MemberRoomIdDTO roomIdDTO)
        {
            var room = await _roomService.GetMemberRoomById(roomIdDTO);

            if (room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [HttpGet("/member/room/domain/{RoomDomain}")]
        public async Task<IActionResult> GetRoomByDomain([FromRoute] MemberRoomDomainDTO roomDomainDTO)
        {
            var room = await _roomService.GetMemberRoomByDomain(roomDomainDTO);

            if (room == null)
                return this.ResultResponse<object>(OperationErrorMessages.RoomNotFound);

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }
    }
}
