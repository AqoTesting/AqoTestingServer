using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member[]> GetMembersByIds(ObjectId[] memberIds);
    }
}
