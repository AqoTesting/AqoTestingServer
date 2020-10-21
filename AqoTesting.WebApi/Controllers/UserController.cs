using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    public class UserController : Controller
    {

        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/user/token")]
        public async Task<IActionResult> GetToken([FromBody] SignInUserDTO userModel)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);


            return this.ResultResponse(OperationErrorMessages.NoError, userModel); // Получим null
        }

        [HttpGet("/user/profile")]
        public async Task<IActionResult> GetProfile()
        {
            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
    }
}
