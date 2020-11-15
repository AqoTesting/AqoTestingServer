using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
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
        public async Task<IActionResult> SignIn([FromBody] UserAPI_SignIn_DTO authData)
        {
            var user = await _userService.GetUserByAuthData(authData);
            if(user == null)
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);

            var userToken = TokenGenerator.GenerateToken(user.Id, Role.User);
            var responseUserToken = new Token_DTO { Token = userToken };

            return this.ResultResponse(OperationErrorMessages.NoError, responseUserToken);
        }

        [HttpPost("/user/signup")]
        public async Task<IActionResult> SignUp([FromBody] UserAPI_SignUp_DTO signUpUserDTO)
        {
            var loginAlreadyTaken = await _userService.GetUserByLogin(signUpUserDTO.Login);
            if(loginAlreadyTaken != null)
                return this.ResultResponse<object>(OperationErrorMessages.LoginAlreadyTaken);

            var emailAlreadyTaken = await _userService.GetUserByEmail(signUpUserDTO.Email);
            if(emailAlreadyTaken != null)
                return this.ResultResponse<object>(OperationErrorMessages.EmailAlreadyTaken);

            var newUser = await _userService.InsertUser(signUpUserDTO);
            var newUserToken = TokenGenerator.GenerateToken(newUser.Id, Role.User);
            var reponseNewUserToken = new Token_DTO { Token = newUserToken };

            return this.ResultResponse(OperationErrorMessages.NoError, reponseNewUserToken);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userService.GetUserById(_workContext.UserId);

            return this.ResultResponse(OperationErrorMessages.NoError, user);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/{UserId}")]
        public async Task<IActionResult> GetUser([FromRoute] UserId_DTO userIdDTO)
        {
            var user = await _userService.GetUserById(userIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, user);
        }
    }
}
