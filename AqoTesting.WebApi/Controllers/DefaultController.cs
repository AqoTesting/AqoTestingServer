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

namespace AqoTestingServer.Controllers
{
    [Produces("application/json")]
    public class DefaultController : Controller
    {

        IExampleService _exampleService;

        public DefaultController(IExampleService exampleService)
        {
            _exampleService = exampleService;
        }

        [HttpPost("/token")]
        public async Task<IActionResult> GetToken([FromBody] LoginUserDTO userModel)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return this.ResultResponse<LoginUserDTO>(OperationErrorMessages.NoError, userModel); // Получим null
        }

        [HttpGet("/value")]
        public async Task<IActionResult> GetApi()
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return this.ResultResponse<object>(OperationErrorMessages.NoError); // Получим null
        }

        [HttpGet("/meow")]
        public async Task<IActionResult> GetMeow()
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            return this.ResultResponse<MeowDTO>(OperationErrorMessages.MayError, await _exampleService.GetMeow());
        }
    }
}
