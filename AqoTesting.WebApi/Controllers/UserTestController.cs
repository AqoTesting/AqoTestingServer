using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Test;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserTestController : Controller
    {
        ITestService _testService;
            
        public UserTestController(ITestService testService)
        {
            _testService = testService;
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpGet("/user/room/{RoomId}/tests")]
        public async Task<IActionResult> GetTests([FromRoute] RoomIdDTO roomIdDTO)
        {
            var tests = await _testService.GetTestsByRoomId(roomIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, tests);
        }

        [Authorize(Roles = "User")]
        [OnlyRoomOwner]
        [HttpGet("/user/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] TestIdDTO testIdDTO)
        {
            var test = await _testService.GetTestById(testIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, test);
        }
    }
}