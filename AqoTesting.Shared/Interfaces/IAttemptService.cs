using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IAttemptService
    {
        #region UserAPI
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(CommonAPI_TestId_DTO testIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(CommonAPI_MemberId_DTO memberIdDTO);
        #endregion

        #region MemberAPI
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttempt();
        Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttemptId();

        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(CommonAPI_MemberId_DTO memberIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByTestId(ObjectId testId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByTestId(CommonAPI_TestId_DTO testIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_Answer(CommonAPI_TestSectionId_DTO sectionIdDTO, CommonAPI_TestQuestionId_DTO questionIdDTO, MemberAPI_CommonTestAnswer_DTO commonAnswerDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(CommonAPI_MemberId_DTO memberIdDTO);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(CommonAPI_AttemptId_DTO attemptIdDTO);
        #endregion

        #region CommonAPI
        Task<(OperationErrorMessages, object)> CommonAPI_FinishAttempt(AttemptsDB_Attempt_DTO attempt);
        #endregion
    }
}
