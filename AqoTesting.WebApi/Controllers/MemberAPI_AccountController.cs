using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_AccountController : Controller
    {

        IMemberService _memberService;
        IRoomService _roomService;
        IWorkContext _workContext;

        public MemberAPI_AccountController(IMemberService memberService, IRoomService roomService, IWorkContext workContext)
        {
            _memberService = memberService;
            _roomService = roomService;
            _workContext = workContext;
        }

        [HttpPost("/member/signin")]
        public async Task<IActionResult> SignIn([FromBody] MemberAPI_SignIn_DTO authData)
        {
            (var errorCode, var token) = await _memberService.MemberAPI_SignIn(authData);

            return this.ResultResponse(errorCode, token);
        }

        //[HttpPost("/member/signup")]
        //public async Task<IActionResult> SignUp([FromBody] MemberAPI_SignUp_DTO signUpMemberDTO)
        //{
        //    (var errorCode, var response) = await _memberService.MemberAPI_SignUp(signUpMemberDTO);

        //    return this.ResultResponse(errorCode, response);
        //}

        [Auth(Role = Role.Member)]
        [HttpGet("/member")]
        public async Task<IActionResult> GetProfile()
        {
            var member = await _memberService.MemberAPI_GetMemberById(_workContext.MemberId);

            return this.ResultResponse(OperationErrorMessages.NoError, member);
        }
    }
}
