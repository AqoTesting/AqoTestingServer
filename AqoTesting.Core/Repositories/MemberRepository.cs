using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        ICacheRepository _redisCache;
        public MemberRepository(ICacheRepository cache)
        {
            _redisCache = cache;
        }

        public async Task<MembersDB_MemberDTO> GetMemberById(ObjectId memberId) =>
            await _redisCache.Get<MembersDB_MemberDTO>($"Member:{memberId}", async () => await MemberWorker.GetMemberById(memberId));

        public async Task<MembersDB_MemberDTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash) =>
            await Task.Run(() => MemberWorker.GetMemberByAuthData(roomId, login, passwordHash));

        public async Task<bool> CheckLoginTaken(ObjectId roomId, string login) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByLogin(roomId, login));
        public async Task<bool> CheckEmailTaken(ObjectId roomId, string email) =>
            await Task.Run(() => MemberWorker.CheckMemberInRoomByEmail(roomId, email));

        public async Task<MembersDB_MemberDTO[]> GetMembersByIds(ObjectId[] memberIds) =>
            await Task.Run(() => MemberWorker.GetMembersByIds(memberIds));

        public async Task<MembersDB_MemberDTO[]> GetMembersByRoomId(ObjectId roomId) =>
            await Task.Run(() => MemberWorker.GetMembersByRoomId(roomId));

        public async Task<MembersDB_MemberDTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash) =>
            await Task.Run(() => MemberWorker.GetMemberByFieldsHash(roomId, fieldsHash));

        public async Task<ObjectId> InsertMember(MembersDB_MemberDTO newMember) =>
            await Task.Run(() => MemberWorker.InsertMember(newMember));

        public async Task ReplaceMember(MembersDB_MemberDTO member)
        {
            await _redisCache.Del($"Member:{member.Id}");

            await Task.Run(() => MemberWorker.ReplaceMember(member));
        }

        public async Task SetTags(ObjectId memberId, MembersDB_TagDTO[] newValue)
        {
            await _redisCache.Del($"Member:{memberId}");
            await MemberWorker.SetProperty(memberId, "Tags", newValue);
        }

        public async Task<bool> SetProperty(ObjectId memberId, string propertyName, object newPropertyValue)
        {
            await _redisCache.Del($"Member:{memberId}");

            return await MemberWorker.SetProperty(memberId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId memberId, Dictionary<string, object> properties)
        {
            await _redisCache.Del($"Member:{memberId}");

            return await MemberWorker.SetProperties(memberId, properties);
        }

        public async Task<bool> Delete(ObjectId memberId)
        {
            var response = await Task.Run(() => MemberWorker.DeleteMember(memberId));
            await _redisCache.Del($"Member:{memberId}");

            return response;
        }
    }
}