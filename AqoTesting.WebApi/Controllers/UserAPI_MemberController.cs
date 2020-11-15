using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserRoomController : Controller
    {
        IMemberService _memberService;
        IWorkContext _workContext;

        public UserRoomController(IMemberService memberService, IWorkContext workContext)
        {
            _workContext = workContext;
            _memberService = memberService;
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}/members")]
        public async Task<IActionResult> GetMembersByRoomId([FromRoute] RoomId_DTO roomIdDTO)
        {
            var members = await _memberService.UserAPI_GetMembersByRoomId(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, members);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPost("/user/room/{RoomId}/member")]
        public async Task<IActionResult> AddMember([FromRoute] RoomId_DTO roomIdDTO, [FromBody] UserAPI_PostMember_DTO addMemberDTO)
        {
            (var errorCode, var response) = await _memberService.UserAPI_MemberManualAdd(roomIdDTO, addMemberDTO);

            return this.ResultResponse(errorCode, response);
        }

        // Это надо переделать, всё завтра, хочу спать
        //[Auth(Role = Role.User)]
        //[OnlyRoomOwner]
        //[HttpDelete("/user/room/{RoomId}/member/{MemberId}")]
        //public async Task<IActionResult> KickMember([FromRoute] RoomId_DTO roomIdDTO, [FromRoute] MemberId_DTO memberTokenDTO)
        //{
        //    await _roomService.UserAPI_RemoveMemberFromRoomById(roomIdDTO, memberTokenDTO);

        //    return this.ResultResponse<object>(OperationErrorMessages.NoError);
        //}
    }
}