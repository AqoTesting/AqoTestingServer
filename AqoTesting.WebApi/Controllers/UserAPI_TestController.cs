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
    public class UserAPI_TestController : Controller
    {
        ITestService _testService;

        public UserAPI_TestController(ITestService testService)
        {
            _testService = testService;
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}/tests")]
        public async Task<IActionResult> GetTests([FromRoute] RoomId_DTO roomIdDTO)
        {
            var tests = await _testService.GetTestsByRoomId(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, tests);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpGet("/user/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] TestId_DTO testIdDTO)
        {
            var test = await _testService.GetTestById(testIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, test);
        }
    }
}