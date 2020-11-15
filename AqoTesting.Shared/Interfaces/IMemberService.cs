using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberService
    {
        Task<(OperationErrorMessages, string)> MemberAPI_SignIn(MemberAPI_SignIn_DTO signInDTO);

        Task<UserAPI_GetMember_DTO> UserAPI_GetMemberById(ObjectId memberId);
        Task<UserAPI_GetMember_DTO> UserAPI_GetMemberById(MemberId_DTO memberIdDTO);

        Task<UserAPI_GetMembersItem_DTO> UserAPI_GetMembersByRoomId(ObjectId roomId);
        Task<UserAPI_GetMembersItem_DTO> UserAPI_GetMembersByRoomId(RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_MemberManualAdd(ObjectId roomId, UserAPI_PostMember_DTO postMemberDTO);
        Task<(OperationErrorMessages, object)> UserAPI_MemberManualAdd(RoomId_DTO roomIdDTO, UserAPI_PostMember_DTO postMemberDTO);

        Task<MemberAPI_GetProfile_DTO> MemberAPI_GetMemberById(ObjectId memberId);
        Task<MemberAPI_GetProfile_DTO> MemberAPI_GetMemberById(MemberId_DTO memberIdDTO);
    }
}
