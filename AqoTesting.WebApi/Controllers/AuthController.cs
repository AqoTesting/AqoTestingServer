using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("/auth")]
    public class AuthController : Controller
    {

        IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInUserDTO authData)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            var user = await _userService.GetUserByAuthData(authData);

            if (user != null)
            {
                var authorizedUser = _userService.GetAuthorizedUser(user);
                return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
            }
            else
            {
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);
            }
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserDTO signUpUserDTO)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            var loginAlreadyTaken = await _userService.GetUserByLogin(signUpUserDTO.Login);
            if (loginAlreadyTaken != null)
            {
                return this.ResultResponse<object>(OperationErrorMessages.LoginAlreadyTaken);
            }
            var emailAlreadyTaken = await _userService.GetUserByEmail(signUpUserDTO.Email);
            if (emailAlreadyTaken != null)
            {
                return this.ResultResponse<object>(OperationErrorMessages.EmailAlreadyTaken);
            }

            var newUser = await _userService.InsertUser(signUpUserDTO);

            var authorizedUser = _userService.GetAuthorizedUser(newUser);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        }
    }
}
