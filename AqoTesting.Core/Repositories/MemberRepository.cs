using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public async Task<Member[]> GetMembersByIds(ObjectId[] memberIds) =>
            await Task.Run(() => MemberWorker.GetMembersByIds(memberIds));
    }
}