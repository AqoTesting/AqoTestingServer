using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
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
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptIdDTO attemptIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(CommonAPI_TestIdDTO testIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(CommonAPI_MemberIdDTO memberIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(CommonAPI_AttemptIdDTO attemptIdDTO, bool newValue);
        Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(ObjectId attemptId, CommonAPI_BooleanValueDTO booleanValueDTO);
        Task<(OperationErrorMessages, object)> UserAPI_SetAttemptIgnore(CommonAPI_AttemptIdDTO attemptIdDTO, CommonAPI_BooleanValueDTO booleanValueDTO);

        Task<(OperationErrorMessages, object)> UserAPI_DeleteAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> UserAPI_DeleteAttempt(CommonAPI_AttemptIdDTO attemptIdDTO);
        #endregion

        #region MemberAPI
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttempt(CommonAPI_AttemptIdDTO attemptIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttempt();
        Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttemptResumeInfo();

        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetAttemptsByMemberId(CommonAPI_MemberIdDTO memberIdDTO);

        Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(ObjectId testId, ObjectId memberId);
        Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(CommonAPI_TestIdDTO testIdDTO, ObjectId memberId);
        Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(ObjectId testId, CommonAPI_MemberIdDTO memberIdDTO);
        Task<(OperationErrorMessages, object)> GetAttemptsByTestIdAndMemberId(CommonAPI_TestIdDTO testIdDTO, CommonAPI_MemberIdDTO memberIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_Answer(CommonAPI_TestSectionIdDTO sectionIdDTO, CommonAPI_TestQuestionIdDTO questionIdDTO, MemberAPI_CommonTestAnswerDTO commonAnswerDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptByMemberId(CommonAPI_MemberIdDTO memberIdDTO);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishAttemptById(CommonAPI_AttemptIdDTO attemptIdDTO);
        #endregion

        #region CommonAPI
        Task<(OperationErrorMessages, object)> CommonAPI_FinishAttempt(AttemptsDB_AttemptDTO attempt);
        #endregion
    }
}
