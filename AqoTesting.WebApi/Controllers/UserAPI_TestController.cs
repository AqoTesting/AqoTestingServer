using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
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
        public async Task<IActionResult> GetTests([FromRoute] CommonAPI_RoomId_DTO roomIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestsByRoomId(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPost("/user/room/{RoomId}/test")]
        public async Task<IActionResult> CreateTest([FromRoute] CommonAPI_RoomId_DTO roomIdDTO, [FromBody] UserAPI_PostTest_DTO postTestDTO)
        {
            var (errorCorde, response) = await _testService.UserAPI_CreateTest(roomIdDTO, postTestDTO);

            return this.ResultResponse(errorCorde, response);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPut("/user/test/{TestId}")]
        public async Task<IActionResult> EditTest([FromRoute] CommonAPI_TestId_DTO testIdDTO, [FromBody] UserAPI_PostTest_DTO postTestDTO)
        {
            var (errorCorde, response) = await _testService.UserAPI_EditTest(testIdDTO, postTestDTO);

            return this.ResultResponse(errorCorde, response);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpPatch("/user/test/{TestId}/sections")]
        public async Task<IActionResult> EditSections([FromRoute] CommonAPI_TestId_DTO testIdDTO, [FromBody] UserAPI_PostSections_DTO postSectionDTOs)
        {
            var (errorCode, response) = await _testService.UserAPI_EditSections(testIdDTO, postSectionDTOs);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [OnlyRoomOwner]
        [HttpDelete("/user/test/{TestId}")]
        public async Task<IActionResult> DeleteTest([FromRoute] CommonAPI_TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_DeleteTest(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}