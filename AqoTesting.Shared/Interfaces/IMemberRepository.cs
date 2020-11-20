using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<MembersDB_Member_DTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash);
        
        Task<bool> CheckLoginTaken(ObjectId roomId, string login);
        Task<bool> CheckEmailTaken(ObjectId roomId, string email);

        Task<MembersDB_Member_DTO> GetMemberById(ObjectId memberId);

        Task<MembersDB_Member_DTO[]> GetMembersByIds(ObjectId[] memberIds);

        Task<MembersDB_Member_DTO[]> GetMembersByRoomId(ObjectId roomId);

        Task<MembersDB_Member_DTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash);

        Task<ObjectId> InsertMember(MembersDB_Member_DTO newMember);

        Task ReplaceMember(MembersDB_Member_DTO updatedMember);

        Task<bool> SetIsRegistered(ObjectId memberId, bool newValue);
        Task<bool> SetIsApproved(ObjectId memberId, bool newValue);
    }
}
