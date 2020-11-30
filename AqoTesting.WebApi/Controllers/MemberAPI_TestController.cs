using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.MemberAPI;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class MemberAPI_TestController : Controller
    {
        IWorkContext _workContext;
        ITestService _testService;

        public MemberAPI_TestController(ITestService testService, IWorkContext workContext)
        {
            _testService = testService;
            _workContext = workContext;
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [HttpGet("/member/tests")]
        public async Task<IActionResult> GetTests()
        {
            var (errorCode, response) = await _testService.MemberAPI_GetTestsByRoomId(_workContext.RoomId.Value);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [MemberAPI_TestAccess]
        [HttpGet("/member/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.MemberAPI_GetTestById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.Member)]
        [MemberAPI_IsApproved]
        [MemberAPI_TestAccess]
        [CommonAPI_CheckAttemptsTime]
        [MemberAPI_HasNoActiveAttempt]
        [HttpGet("/member/test/{TestId}/begin")]
        public async Task<IActionResult> BeginTest([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.MemberAPI_BeginTest(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}