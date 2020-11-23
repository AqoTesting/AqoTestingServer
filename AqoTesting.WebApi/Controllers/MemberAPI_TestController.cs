using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
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

        [Auth(Role = Role.Member)]
        //[MemberIsRegistered]
        [MemberIsApproved]
        [HttpGet("/member/tests")]
        public async Task<IActionResult> GetTests()
        {
            var (errorCode, response) = await _testService.MemberAPI_GetTestsByRoomId(_workContext.RoomId);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.Member)]
        //[MemberIsRegistered]
        [MemberIsApproved]
        [HttpGet("/member/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.MemberAPI_GetTestById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}