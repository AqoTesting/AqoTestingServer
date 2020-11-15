using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<(bool, ObjectId)> GetMemberIdByAuthData(string login, byte[] passwordHash);

        Task<bool> CheckFieldsHashExists(ObjectId roomId, byte[] fieldsHash);

        Task<Member> GetMemberById(ObjectId memberId);

        Task<Member[]> GetMembersByIds(ObjectId[] memberIds);

        Task<Member[]> GetMembersByRoomId(ObjectId roomId);

        Task<ObjectId> InsertMember(Member newMember);
    }
}
