using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserAPI_AccountController : Controller
    {

        IUserService _userService;
        IWorkContext _workContext;

        public UserAPI_AccountController(IUserService userService, IWorkContext workContext)
        {
            _userService = userService;
            _workContext = workContext;
        }

        [HttpPost("/user/signin")]
        public async Task<IActionResult> SignIn([FromBody] UserAPI_SignInDTO signInDTO)
        {
            var (errorCode, response) = await _userService.UserAPI_SignIn(signInDTO);

            return this.ResultResponse(errorCode, response);
        }

        [HttpPost("/user/signup")]
        public async Task<IActionResult> SignUp([FromBody] UserAPI_SignUpDTO signUpDTO)
        {
            var (errorCode, respone) = await _userService.UserAPI_SignUp(signUpDTO);
            
            return this.ResultResponse(errorCode, respone);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [HttpGet("/user")]
        public async Task<IActionResult> GetProfile()
        {
            var (errorCode, respone) = await _userService.UserAPI_GetUserById(_workContext.UserId.Value);

            return this.ResultResponse(errorCode, respone);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [HttpGet("/user/{UserId}")]
        public async Task<IActionResult> GetUser([FromRoute] CommonAPI_UserIdDTO userIdDTO)
        {
            var (errorCode, respone) = await _userService.UserAPI_GetUserById(userIdDTO);

            return this.ResultResponse(errorCode, respone);
        }
    }
}
