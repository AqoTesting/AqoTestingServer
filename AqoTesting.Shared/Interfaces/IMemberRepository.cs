using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member[]> GetMembersByIds(ObjectId[] memberIds);

        Task<Member> GetMemberByAuthData(string login, byte[] passwordHash);

        Task<Member> GetMemberByFields(ObjectId roomId, Dictionary<string, string> fields);
    }
}
