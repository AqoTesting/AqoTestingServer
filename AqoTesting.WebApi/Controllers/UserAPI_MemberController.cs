using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserAPI_MemberController : Controller
    {
        IMemberService _memberService;

        public UserAPI_MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpGet("/user/room/{RoomId}/members")]
        public async Task<IActionResult> GetMembersByRoomId([FromRoute] CommonAPI_RoomId_DTO roomIdDTO)
        {
            var (errorCode, members) = await _memberService.UserAPI_GetMembersByRoomId(roomIdDTO);

            return this.ResultResponse(errorCode, members);
        }

        [Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpPost("/user/room/{RoomId}/member")]
        public async Task<IActionResult> AddMember([FromRoute] CommonAPI_RoomId_DTO roomIdDTO, [FromBody] UserAPI_PostMember_DTO addMemberDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_ManualMemberAdd(roomIdDTO, addMemberDTO);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpDelete("/user/member/{MemberId}")]
        public async Task<IActionResult> KickMember([FromRoute] CommonAPI_MemberId_DTO memberIdDTO)
        {
            await _memberService.UserAPI_Delete(memberIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }

        [Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpPatch("/user/member/{MemberId}/unregister")]
        public async Task<IActionResult> Unregister([FromRoute] CommonAPI_MemberId_DTO memberIdDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_Unregister(memberIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpPatch("/user/member/{MemberId}/approve")]
        public async Task<IActionResult> Approve([FromRoute] CommonAPI_MemberId_DTO memberIdDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_Approve(memberIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}