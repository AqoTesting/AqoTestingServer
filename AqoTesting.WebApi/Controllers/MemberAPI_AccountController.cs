using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using AqoTesting.WebApi.Attributes.CommonAPI;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_AccountController : Controller
    {

        IMemberService _memberService;
        IWorkContext _workContext;

        public MemberAPI_AccountController(IMemberService memberService, IWorkContext workContext)
        {
            _memberService = memberService;
            _workContext = workContext;
        }

        [HttpPost("/member/signin")]
        public async Task<IActionResult> SignIn([FromBody] MemberAPI_SignIn_DTO authData)
        {
            (var errorCode, var response) = await _memberService.MemberAPI_SignIn(authData);

            return this.ResultResponse(errorCode, response);
        }

        [HttpPost("/member/signup")]
        public async Task<IActionResult> SignUp([FromBody] MemberAPI_SignUp_DTO signUpDTO)
        {
            (var errorCode, var response) = await _memberService.MemberAPI_SignUp(signUpDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [HttpGet("/member")]
        public async Task<IActionResult> GetProfile()
        {
            (var errorCode, var response) = await _memberService.MemberAPI_GetMemberById(_workContext.MemberId.Value);

            return this.ResultResponse(errorCode, response);
        }
    }
}
