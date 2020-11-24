using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserAPI_AttemptController : Controller
    {
        IAttemptService _attemptService;

        public UserAPI_AttemptController(IAttemptService attemptService)
        {
            _attemptService = attemptService;
        }

        [Auth(Role = Role.User)]
        //[UserAPI_AttemptAccess]
        [HttpGet("/user/attempt/{AttemptId}")]
        public async Task<IActionResult> GetAttempt([FromRoute] CommonAPI_AttemptId_DTO attemptIdDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_GetAttempt(attemptIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}