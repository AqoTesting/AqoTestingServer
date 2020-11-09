using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {

        IWorkContext _workContext;
        IUserService _userService;

        public UserController(IWorkContext workContext, IUserService userService)
        {
            _workContext = workContext;
            _userService = userService;
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user")]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _userService.GetUserById(_workContext.UserId);

            return this.ResultResponse(OperationErrorMessages.NoError, user);
        }

        [Auth(Role = Role.User)]
        [HttpGet("/user/{UserId}")]
        public async Task<IActionResult> GetUser([FromRoute] UserIdDTO userIdDTO)
        {
            var user = await _userService.GetUserById(userIdDTO);

            return this.ResultResponse(OperationErrorMessages.NoError, user);
        }
    }
}
