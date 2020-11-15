using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public async Task<Member> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash) =>
            await Task.Run(() => MemberWorker.GetMemberByAuthData(roomId, login, passwordHash));

        public async Task<bool> CheckLoginTaken(ObjectId roomId, string login) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByLogin(roomId, login));
        public async Task<bool> CheckEmailTaken(ObjectId roomId, string email) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByEmail(roomId, email));

        public async Task<Member> GetMemberById(ObjectId memberId) =>
            await Task.Run(() => MemberWorker.GetMemberById(memberId));

        public async Task<Member[]> GetMembersByIds(ObjectId[] memberIds) =>
            await Task.Run(() => MemberWorker.GetMembersByIds(memberIds));

        public async Task<Member[]> GetMembersByRoomId(ObjectId roomId) =>
            await Task.Run(() => MemberWorker.GetMembersFromRoom(roomId));

        public async Task<Member> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash) =>
            await Task.Run(() => MemberWorker.GetMemberByFieldsHash(roomId, fieldsHash));

        public async Task<ObjectId> InsertMember(Member newMember) =>
            await Task.Run(() => MemberWorker.InsertMember(newMember));

        public async Task ReplaceMember(Member member) =>
            await Task.Run(() => MemberWorker.ReplaceMember(member));
    }
}