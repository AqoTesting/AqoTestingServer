using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
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

        [Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [CommonAPI_CheckAttemptTime]
        [HttpGet("/member/attempt/active")]
        public async Task<IActionResult> GetActiveAttempt()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetActiveAttempt();

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [MemberAPI_HasActiveAttempt]
        [CommonAPI_CheckAttemptTime]
        [HttpPost("/member/attempt/active/section/{SectionId}/question/{QuestionId}/answer")]
        public async Task<IActionResult> Answer([FromRoute] CommonAPI_TestSectionId_DTO sectionIdDTO, [FromRoute] CommonAPI_TestQuestionId_DTO questionIdDTO, [FromBody] MemberAPI_CommonTestAnswer_DTO answerDTO)
        {
            var (errorCode, response) = await _attemptService.MemberAPI_Answer(sectionIdDTO, questionIdDTO, answerDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}