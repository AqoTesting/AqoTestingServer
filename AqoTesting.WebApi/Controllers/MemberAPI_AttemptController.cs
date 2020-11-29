using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.MemberAPI;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_AttemptController : Controller
    {
        IWorkContext _workContext;
        IAttemptService _attemptService;

        public MemberAPI_AttemptController(IAttemptService attemptService, IWorkContext workContext)
        {
            _attemptService = attemptService;
            _workContext = workContext;
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_AttemptAccess]
        [HttpGet("/member/attempt/{AttemptId}")]
        public async Task<IActionResult> GetAttempt([FromRoute] CommonAPI_AttemptId_DTO attemptIdDTO)
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetAttempt(attemptIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [HttpGet("/member/attempts")]
        public async Task<IActionResult> GetMemberAttempts()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetAttemptsByMemberId(_workContext.MemberId.Value);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_TestAccess]
        [HttpGet("/member/test/{TestId}/attempts")]
        public async Task<IActionResult> GetAttemptsByTestId([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetAttemptsByTestId(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_HasActiveAttempt]
        [HttpGet("/member/attempt/active")]
        public async Task<IActionResult> GetActiveAttempt()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetActiveAttempt();

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_HasActiveAttempt]
        [HttpGet("/member/attempt/active/id")]
        public async Task<IActionResult> GetActiveAttemptId()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetActiveAttemptId();

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_HasActiveAttempt]
        [HttpPatch("/member/attempt/active/section/{SectionId}/question/{QuestionId}/answer")]
        public async Task<IActionResult> Answer([FromRoute] CommonAPI_TestSectionId_DTO sectionIdDTO, [FromRoute] CommonAPI_TestQuestionId_DTO questionIdDTO, [FromBody] MemberAPI_CommonTestAnswer_DTO answerDTO)
        {
            var (errorCode, response) = await _attemptService.MemberAPI_Answer(sectionIdDTO, questionIdDTO, answerDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_HasActiveAttempt]
        [HttpPatch("/member/attempt/active/finish")]
        public async Task<IActionResult> Finish()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_FinishAttemptByMemberId(_workContext.MemberId.Value);

            return this.ResultResponse(errorCode, response);
        }
    }
}