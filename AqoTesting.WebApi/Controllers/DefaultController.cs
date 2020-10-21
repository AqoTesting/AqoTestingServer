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

namespace AqoTestingServer.Controllers
{
    [Produces("application/json")]
    public class DefaultController : Controller
    {

        IExampleService _exampleService;
        IUserService _userService;

        public DefaultController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        //# Так ли это должно работать? Скорее всего - нет
        public DefaultController(IUserService userService)
        {
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

            //# Чё как тут ошибку передать я всё равно не понял
            return this.ResultResponse<object>(OperationErrorMessages.NoError, await _userService.SignIn(authData.Login, authData.Password));
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
