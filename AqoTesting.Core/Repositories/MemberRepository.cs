using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        ICacheRepository _cache;
        public MemberRepository(ICacheRepository cache)
        {
            _cache = cache;
        }

        public async Task<MembersDB_Member_DTO> GetMemberById(ObjectId memberId) =>
            await _cache.Get<MembersDB_Member_DTO>($"Member:{memberId}", async () => await MemberWorker.GetMemberById(memberId));

        public async Task<MembersDB_Member_DTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash) =>
            await Task.Run(() => MemberWorker.GetMemberByAuthData(roomId, login, passwordHash));

        public async Task<bool> CheckLoginTaken(ObjectId roomId, string login) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByLogin(roomId, login));
        public async Task<bool> CheckEmailTaken(ObjectId roomId, string email) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByEmail(roomId, email));

        public async Task<MembersDB_Member_DTO[]> GetMembersByIds(ObjectId[] memberIds) =>
            await Task.Run(() => MemberWorker.GetMembersByIds(memberIds));

        public async Task<MembersDB_Member_DTO[]> GetMembersByRoomId(ObjectId roomId) =>
            await Task.Run(() => MemberWorker.GetMembersFromRoom(roomId));

        public async Task<MembersDB_Member_DTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash) =>
            await Task.Run(() => MemberWorker.GetMemberByFieldsHash(roomId, fieldsHash));

        public async Task<ObjectId> InsertMember(MembersDB_Member_DTO newMember) =>
            await Task.Run(() => MemberWorker.InsertMember(newMember));

        public async Task ReplaceMember(MembersDB_Member_DTO member)
        {
            await Task.Run(() => MemberWorker.ReplaceMember(member));
            await _cache.Del($"Member:{member.Id}");
        }

        public async Task<bool> SetIsRegistered(ObjectId memberId, bool newValue)
        {
            var response = await Task.Run(() => MemberWorker.SetIsRegistered(memberId, newValue));
            await _cache.Del($"Member:{memberId}");

            return response;
        }

        public async Task<bool> SetIsApproved(ObjectId memberId, bool newValue)
        {
            var response = await Task.Run(() => MemberWorker.SetIsApproved(memberId, newValue));
            await _cache.Del($"Member:{memberId}");

            return response;
        }

        public async Task<bool> Delete(ObjectId roomId, ObjectId memberId)
        {
            var response = await Task.Run(() => RoomWorker.RemoveMemberFromRoomById(roomId, memberId));
            await _cache.Del($"Member:{memberId}");

            return response;
        }
    }
}