using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class AuthController : Controller
    {

        IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/auth/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInUserDTO authData)
        {
            var user = await _userService.GetUserByAuthData(authData);

            var authorizedUser = _userService.GetAuthorizedUser(user);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        }

        [HttpPost("/auth/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserDTO signUpUserDTO)
        {
            var loginAlreadyTaken = await _userService.GetUserByLogin(signUpUserDTO.Login);
            if (loginAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.LoginAlreadyTaken);

            var emailAlreadyTaken = await _userService.GetUserByEmail(signUpUserDTO.Email);
            if (emailAlreadyTaken != null)
                throw new ResultException(OperationErrorMessages.EmailAlreadyTaken);

            var newUser = await _userService.InsertUser(signUpUserDTO);

            var authorizedUser = _userService.GetAuthorizedUser(newUser);

            return this.ResultResponse(OperationErrorMessages.NoError, authorizedUser);
        }
    }
}
