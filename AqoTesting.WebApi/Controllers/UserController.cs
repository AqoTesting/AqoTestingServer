using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.BD.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {

        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/auth/signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInUserDTO authData) {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            User user = await _userService.GetUserByAuthData(authData);

            if (user != null) {
                var userDto = await _userService.GetAuthorizedUser(user);
                return this.ResultResponse(OperationErrorMessages.NoError, userDto);
            } else {
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);
            }
        }

        [HttpPost("/auth/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserDTO userData) {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return null;
        }

        [Authorize(Roles = "User")]
        [HttpGet("/profile")]
        public async Task<IActionResult> GetProfile() {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            //ObjectId.Parse(User.FindFirst("Id").Value);

            return this.ResultResponse(OperationErrorMessages.NoError, User.FindFirst("Id").Value);
        }
    }
}
