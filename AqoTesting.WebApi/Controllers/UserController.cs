using System.Threading.Tasks;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
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

        [Authorize(Roles = "User")]
        [HttpGet("/user")]
        public async Task<IActionResult> GetProfile()
        {
            throw new ResultException(OperationErrorMessages.LoginAlreadyTaken);

            //return this.ResultResponse(OperationErrorMessages.NoError, _workContext.UserId);
        }
    }
}
