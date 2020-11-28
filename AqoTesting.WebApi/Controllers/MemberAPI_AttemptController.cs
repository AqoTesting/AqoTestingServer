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
        [HttpGet("/member/attempt/active")]
        public async Task<IActionResult> GetActiveAttempt()
        {
            var (errorCode, response) = await _attemptService.MemberAPI_GetActiveAttempt();

            return this.ResultResponse(errorCode, response);
        }
    }
}