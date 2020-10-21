using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AqoTesting.Shared.Models;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.DTOs.API.Users;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Cryptography;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.BD.Users;
using AqoTesting.Shared.Utils;

namespace AqoTestingServer.Controllers
{
    [Produces("application/json")]
    public class DefaultController : Controller
    {

        IExampleService _exampleService;
        IUserService _userService;

        public DefaultController(IExampleService exampleService, IUserService userService)
        {
            _exampleService = exampleService;
            _userService = userService;
        }

        [HttpGet("/value")]
        public async Task<IActionResult> GetApi()
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return this.ResultResponse<object>(OperationErrorMessages.NoError); // Получим null
        }

        [HttpPost("/auth/signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginUserDTO authData)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            User user = MongoIOController.GetUserByData(authData.Login, Sha256.Compute(authData.Password));

            if (user != null) {
                string token = " generateJwtToken() ";

                AuthorizedUserDTO userDto = new AuthorizedUserDTO {
                    Token = token,
                    Login = user.Login,
                    Email = user.Email,
                    Name = user.Name
                };

                return this.ResultResponse<AuthorizedUserDTO>(OperationErrorMessages.NoError, userDto);
            }
            else
            {
                return this.ResultResponse<object>(OperationErrorMessages.WrongAuthData);
            }
        }

        [Authorize]
        [HttpGet("/meow")]
        public async Task<IActionResult> GetMeow()
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return this.ResultResponse<MeowDTO>(OperationErrorMessages.MayError, await _exampleService.GetMeow());
        }
    }
}
