using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserAuthController : Controller
    {

        IUserService _userService;

        public UserAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/user/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInUserDTO authData)
        {
            var user = await _userService.GetUserByAuthData(authData);
            if (user == null)
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);

            var authorizedUser = TokenGenerator.GenerateToken(user.Id);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        }

        [HttpPost("/user/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserDTO signUpUserDTO)
        {
            var loginAlreadyTaken = await _userService.GetUserByLogin(signUpUserDTO.Login);
            if (loginAlreadyTaken != null)
                return this.ResultResponse<object>(OperationErrorMessages.LoginAlreadyTaken);

            var emailAlreadyTaken = await _userService.GetUserByEmail(signUpUserDTO.Email);
            if (emailAlreadyTaken != null)
                return this.ResultResponse<object>(OperationErrorMessages.EmailAlreadyTaken);

            var newUser = await _userService.InsertUser(signUpUserDTO);
            var authorizedUser = TokenGenerator.GenerateToken(newUser.Id);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        }
    }
}
