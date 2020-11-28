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

        Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId);

        Task<ObjectId> InsertAttempt(AttemptsDB_Attempt_DTO newAttempt);
    }
}
