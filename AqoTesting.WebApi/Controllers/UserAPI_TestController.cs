﻿using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.UserAPI;
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

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpGet("/user/room/{RoomId}/tests")]
        public async Task<IActionResult> GetTests([FromRoute] CommonAPI_RoomIdDTO roomIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestsByRoomId(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [HttpGet("/user/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] CommonAPI_TestIdDTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [HttpGet("/user/test/{TestId}/info")]
        public async Task<IActionResult> GetTestInfo([FromRoute] CommonAPI_TestIdDTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestInfoById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }


        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_RoomAccess]
        [HttpPost("/user/room/{RoomId}/test")]
        public async Task<IActionResult> CreateTest([FromRoute] CommonAPI_RoomIdDTO roomIdDTO, [FromBody] UserAPI_PostTestDTO postTestDTO)
        {
            var (errorCorde, response) = await _testService.UserAPI_CreateTest(roomIdDTO, postTestDTO);

            return this.ResultResponse(errorCorde, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [HttpPut("/user/test/{TestId}")]
        public async Task<IActionResult> EditTest([FromRoute] CommonAPI_TestIdDTO testIdDTO, [FromBody] UserAPI_PostTestDTO postTestDTO)
        {
            var (errorCorde, response) = await _testService.UserAPI_EditTest(testIdDTO, postTestDTO);

            return this.ResultResponse(errorCorde, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [HttpPatch("/user/test/{TestId}/sections")]
        public async Task<IActionResult> EditSections([FromRoute] CommonAPI_TestIdDTO testIdDTO, [FromBody] UserAPI_PostTestSectionsDTO postSectionDTOs)
        {
            var (errorCode, response) = await _testService.UserAPI_EditSections(testIdDTO, postSectionDTOs);

            return this.ResultResponse(errorCode, response);
        }

        [CommonAPI_Auth(Role = Role.User)]
        [UserAPI_TestAccess]
        [HttpDelete("/user/test/{TestId}")]
        public async Task<IActionResult> DeleteTest([FromRoute] CommonAPI_TestIdDTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_DeleteTest(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}