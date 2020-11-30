using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<MembersDB_MemberDTO> GetMemberById(ObjectId memberId);

        Task<MembersDB_MemberDTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash);

        Task<bool> CheckLoginTaken(ObjectId roomId, string login);
        Task<bool> CheckEmailTaken(ObjectId roomId, string email);

        Task<MembersDB_MemberDTO[]> GetMembersByIds(ObjectId[] memberIds);
        Task<MembersDB_MemberDTO[]> GetMembersByRoomId(ObjectId roomId);

        Task<MembersDB_MemberDTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash);

        Task<ObjectId> InsertMember(MembersDB_MemberDTO newMember);

        Task ReplaceMember(MembersDB_MemberDTO member);

        Task<bool> SetProperty(ObjectId memberId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId memberId, Dictionary<string, object> properties);

        Task<bool> Delete(ObjectId memberId);
    }
}
