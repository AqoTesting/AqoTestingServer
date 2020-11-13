using System.Collections.Generic;
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

        public async Task<Member> GetMemberByAuthData(string login, byte[] passwordHash) =>
            await Task.Run(() => MemberWorker.GetMemberByAuthData(login, passwordHash));

        public async Task<Member> GetMemberByFields(ObjectId roomId, Dictionary<string, string> fields) =>
            await Task.Run(() => MemberWorker.GetMemberByFields(roomId, fields));
    }
}