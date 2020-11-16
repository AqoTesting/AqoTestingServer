﻿using System.Threading.Tasks;
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
            var (errorCode, response) = await _testService.UserAPI_GetTestsByRoomId(roomIdDTO);

            return this.ResultResponse(errorCode, response);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/test/{TestId}")]
        public async Task<IActionResult> GetTest([FromRoute] TestId_DTO testIdDTO)
        {
            var (errorCode, response) = await _testService.UserAPI_GetTestById(testIdDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}