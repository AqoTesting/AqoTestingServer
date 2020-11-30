using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.UserAPI;
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

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpGet("/user/room/{RoomId}/members")]
        public async Task<IActionResult> GetMembersByRoomId([FromRoute] CommonAPI_RoomIdDTO roomIdDTO)
        {
            var (errorCode, members) = await _memberService.UserAPI_GetMembersByRoomId(roomIdDTO);

            return this.ResultResponse(errorCode, members);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpPost("/user/room/{RoomId}/member")]
        public async Task<IActionResult> AddMember([FromRoute] CommonAPI_RoomIdDTO roomIdDTO, [FromBody] UserAPI_PostMemberDTO addMemberDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_ManualMemberAdd(roomIdDTO, addMemberDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpPatch("/user/member/{MemberId}/tags")]
        public async Task<IActionResult> SetMemberTags([FromRoute] CommonAPI_MemberIdDTO memberIdDTO, [FromBody] UserAPI_PostMemberTagsDTO postTagsDTO)
        {
            await _memberService.UserAPI_SetMemberTags(memberIdDTO, postTagsDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpPatch("/user/member/{MemberId}/approve")]
        public async Task<IActionResult> Approve([FromRoute] CommonAPI_MemberIdDTO memberIdDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_Approve(memberIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpPatch("/user/member/{MemberId}/unregister")]
        public async Task<IActionResult> Unregister([FromRoute] CommonAPI_MemberIdDTO memberIdDTO)
        {
            var (errorCode, response) = await _memberService.UserAPI_Unregister(memberIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_MemberAccess]
        [HttpDelete("/user/member/{MemberId}")]
        public async Task<IActionResult> DeleteMember([FromRoute] CommonAPI_MemberIdDTO memberIdDTO)
        {
            await _memberService.UserAPI_Delete(memberIdDTO);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
    }
}