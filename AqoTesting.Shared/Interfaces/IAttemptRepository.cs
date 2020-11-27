using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Attempts;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IAttemptRepository
    {
        Task<AttemptsDB_Attempt_DTO> GetAttemptById(ObjectId attemptId);

        Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestId(ObjectId testId);

        Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByMemberId(ObjectId memberId);
    }
}
