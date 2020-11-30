using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(CommonAPI_MemberId_DTO memberIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(ObjectId roomId, UserAPI_PostMember_DTO postMemberDTO);
        Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostMember_DTO postMemberDTO);

        Task<(OperationErrorMessages, object)> UserAPI_Unregister(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_Unregister(CommonAPI_MemberId_DTO memberIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_Approve(ObjectId memberId);
        Task<(OperationErrorMessages, object)> UserAPI_Approve(CommonAPI_MemberId_DTO memberIdDTO);

        Task<OperationErrorMessages> UserAPI_Delete(ObjectId memberId);
        Task<OperationErrorMessages> UserAPI_Delete(CommonAPI_MemberId_DTO memberIdDTO);


        Task<(OperationErrorMessages, object)> MemberAPI_SignIn(MemberAPI_SignIn_DTO signInDTO);
        Task<(OperationErrorMessages, object)> MemberAPI_SignUp(MemberAPI_SignUp_DTO signUpDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(ObjectId memberId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(CommonAPI_MemberId_DTO memberIdDTO);
    }
}
