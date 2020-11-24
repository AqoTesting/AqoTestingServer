using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IAttemptService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO);
    }
}
