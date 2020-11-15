using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public async Task<(bool, ObjectId)> GetMemberIdByAuthData(string login, byte[] passwordHash) =>
            await Task.Run(() => MemberWorker.GetMemberIdByAuthData(login, passwordHash));

        public async Task<bool> CheckFieldsHashExists(ObjectId roomId, byte[] fieldsHash) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByFieldsHash(roomId, fieldsHash));

        public async Task<Member> GetMemberById(ObjectId memberId) =>
            await Task.Run(() => MemberWorker.GetMemberById(memberId));

        public async Task<Member[]> GetMembersByIds(ObjectId[] memberIds) =>
            await Task.Run(() => MemberWorker.GetMembersByIds(memberIds));

        public async Task<Member[]> GetMembersByRoomId(ObjectId roomId) =>
            await Task.Run(() => MemberWorker.GetMembersFromRoom(roomId));

        public async Task<ObjectId> InsertMember(Member newMember) =>
            await Task.Run(() => MemberWorker.InsertMember(newMember));
    }
}