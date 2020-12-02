using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.InternalAPI;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AqoTesting.WebApi.Attributes.CommonAPI;
using AqoTesting.WebApi.Attributes.UserAPI;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class InternalAPI_ValidationController : Controller
    {
        IValidationService _validationService;

        public InternalAPI_ValidationController(IValidationService validationService)
        {
            _validationService = validationService;
        }

        [InternalAPI_Access]
        [HttpPost("/internal/fileUploadUserAccess")]
        public async Task<IActionResult> FileUploadAccess([FromBody] InternalAPI_FileUploadUserAccessDTO fileUploadUserAccessDTO)
        {
            var (errorCode, response) = await _validationService.InternalAPI_FileUploadUserAccess(fileUploadUserAccessDTO);

            return this.ResultResponse(errorCode, response);
        }
    }
}
