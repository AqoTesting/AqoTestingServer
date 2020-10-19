using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AqoTesting.Core.Models;
using AqoTesting.Core.Enums;
using AqoTesting.Core.DTOs.API;
using AqoTesting.Core.Interfaces;

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

        [HttpGet("/value")]
        public async Task<IActionResult> GetApi()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return this.ResultResponse<bool>(OperationErrorMessages.NoError, true);
        }

        [HttpGet("/meow")]
        public async Task<IActionResult> GetMeow()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return this.ResultResponse<MeowDTO>(OperationErrorMessages.MayError, await _exampleService.GetMeow());
        }
    }
}
