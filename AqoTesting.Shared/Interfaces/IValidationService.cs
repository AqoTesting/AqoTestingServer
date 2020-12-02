using AqoTesting.Shared.DTOs.API.InternalAPI;
using AqoTesting.Shared.Enums;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IValidationService
    {
        Task<(OperationErrorMessages, object)> InternalAPI_FileUploadUserAccess(InternalAPI_FileUploadUserAccessDTO fileUploadUserAccessDTO);
    }
}
