using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.UserAPI;
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

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_AttemptAccess]
        [CommonAPI_CheckAttemptsTime]
        [HttpGet("/user/attempt/{AttemptId}")]
        public async Task<IActionResult> GetAttempt([FromRoute] CommonAPI_AttemptId_DTO attemptIdDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_GetAttempt(attemptIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [CommonAPI_CheckAttemptsTime]
        [HttpGet("/user/test/{TestId}/attempts")]
        public async Task<IActionResult> GetAttemptsByTestId([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_GetAttemptsByTestId(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [CommonAPI_CheckAttemptsTime]
        [HttpGet("/user/member/{MemberId}/attempts")]
        public async Task<IActionResult> GetAttemptsByMemberId([FromRoute] CommonAPI_MemberId_DTO memberIdDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_GetAttemptsByMemberId(memberIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_AttemptAccess]
        [CommonAPI_CheckAttemptsTime]
        [HttpPatch("/user/attempt/{AttemptId}/ignore")]
        public async Task<IActionResult> DeleteAttempt([FromRoute] CommonAPI_AttemptId_DTO attemptIdDTO, [FromBody] CommonAPI_BooleanValue_DTO booleanValueDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_SetAttemptIgnore(attemptIdDTO, booleanValueDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_AttemptAccess]
        [CommonAPI_CheckAttemptsTime]
        [HttpDelete("/user/attempt/{AttemptId}")]
        public async Task<IActionResult> DeleteAttempt([FromRoute] CommonAPI_AttemptId_DTO attemptIdDTO)
        {
            var (errorCode, response) = await _attemptService.UserAPI_DeleteAttempt(attemptIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}