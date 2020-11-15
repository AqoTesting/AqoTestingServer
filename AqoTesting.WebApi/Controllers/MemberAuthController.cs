using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AqoTesting.Core.Services;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAuthController : Controller
    {

        IMemberService _memberService;
        IRoomService _roomService;

        public MemberAuthController(IMemberService memberService, IRoomService roomService)
        {
            _memberService = memberService;
            _roomService = roomService;
        }

        [HttpPost("/member/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInMemberDTO authData)
        {
            var member = await _memberService.GetMemberByAuthData(authData);
            if(member == null)
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);

            var authorizedMember = TokenGenerator.GenerateToken(member.Id, member.RoomId, member.IsChecked);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedMember);
        }

        [HttpPost("/member/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpMemberDTO signUpMemberDTO)
        {
            (var errorCode, var response) = await _memberService.MemberAuth(signUpMemberDTO);

            return this.ResultResponse(errorCode, response);
        }

        //[HttpPost("/auth/signup")]
        //public async Task<IActionResult> SignUp([FromBody] SignUpUserDTO signUpUserDTO)
        //{
        //    var loginAlreadyTaken = await _userService.GetUserByLogin(signUpUserDTO.Login);
        //    if (loginAlreadyTaken != null)
        //        throw new ResultException(OperationErrorMessages.LoginAlreadyTaken);

        //    var emailAlreadyTaken = await _userService.GetUserByEmail(signUpUserDTO.Email);
        //    if (emailAlreadyTaken != null)
        //        throw new ResultException(OperationErrorMessages.EmailAlreadyTaken);

        //    var newUser = await _userService.InsertUser(signUpUserDTO);

        //    var authorizedUser = _userService.GenerateToken(newUser.Id, Role.User);

        //    return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        //}
    }
}
