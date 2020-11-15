using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash);
        
        Task<bool> CheckLoginTaken(ObjectId roomId, string login);
        Task<bool> CheckEmailTaken(ObjectId roomId, string email);

        Task<Member> GetMemberById(ObjectId memberId);

        Task<Member[]> GetMembersByIds(ObjectId[] memberIds);

        Task<Member[]> GetMembersByRoomId(ObjectId roomId);

        Task<Member> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash);

        Task<ObjectId> InsertMember(Member newMember);

        Task ReplaceMember(Member updatedMember);
    }
}
