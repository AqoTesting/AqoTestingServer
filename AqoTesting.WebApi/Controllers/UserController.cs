using System;
using System.Threading.Tasks;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        [HttpPost("/auth/signup")]
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

        [Authorize(Roles = "User")]
        [HttpGet("/profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            //ObjectId.Parse(User.FindFirst("Id").Value);

            return this.ResultResponse(OperationErrorMessages.NoError, User.FindFirst("Id").Value);
        }
    }
}
