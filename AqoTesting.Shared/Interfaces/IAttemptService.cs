using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IAttemptService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(ObjectId attemptId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttempt(CommonAPI_AttemptId_DTO attemptIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByTestId(CommonAPI_TestId_DTO testIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_GetAttemptsByMemberId(CommonAPI_MemberId_DTO memberIdDTO);


        Task<(OperationErrorMessages, object)> MemberAPI_GetActiveAttempt();

        Task<(OperationErrorMessages, object)> MemberAPI_Answer(CommonAPI_TestSectionId_DTO sectionIdDTO, CommonAPI_TestQuestionId_DTO questionIdDTO, MemberAPI_CommonTestAnswer_DTO commonAnswerDTO);
        Task<(OperationErrorMessages, object)> MemberAPI_FinishCurrentAttempt();
    }
}
