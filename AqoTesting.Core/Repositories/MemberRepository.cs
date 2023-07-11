using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;


namespace AqoTesting.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public MemberRepository()
        {
        }

        public async Task<MembersDB_MemberDTO> GetMemberById(ObjectId memberId) =>
            await MemberWorker.GetMemberById(memberId);

        public async Task<MembersDB_MemberDTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash) =>
            await MemberWorker.GetMemberByAuthData(roomId, login, passwordHash);


        public async Task<bool> CheckLoginTaken(ObjectId roomId, string login) =>
            await MemberWorker.CheckMemberInRoomByLogin(roomId, login);
        public async Task<bool> CheckEmailTaken(ObjectId roomId, string email) =>
            await MemberWorker.CheckMemberInRoomByEmail(roomId, email);


        public async Task<MembersDB_MemberDTO[]> GetMembersByIds(ObjectId[] memberIds) =>
            await MemberWorker.GetMembersByIds(memberIds);

        public async Task<MembersDB_MemberDTO[]> GetMembersByRoomId(ObjectId roomId) =>
            await MemberWorker.GetMembersByRoomId(roomId);

        public async Task<MembersDB_MemberDTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash) =>
            await MemberWorker.GetMemberByFieldsHash(roomId, fieldsHash);


        public async Task<ObjectId> InsertMember(MembersDB_MemberDTO newMember) =>
            await MemberWorker.InsertMember(newMember);

        public async Task ReplaceMember(MembersDB_MemberDTO member) =>
            await MemberWorker.ReplaceMember(member);


        public async Task SetTags(ObjectId memberId, MembersDB_TagDTO[] newValue) =>
            await MemberWorker.SetProperty(memberId, "Tags", newValue);

        public async Task<bool> SetProperty(ObjectId memberId, string propertyName, object newPropertyValue) =>
            await MemberWorker.SetProperty(memberId, propertyName, newPropertyValue);

        public async Task<bool> SetProperties(ObjectId memberId, Dictionary<string, object> properties) =>
            await MemberWorker.SetProperties(memberId, properties);


        public async Task<bool> Delete(ObjectId memberId) =>
            await MemberWorker.DeleteMember(memberId);

        public async Task<long> DeleteMembersByRoomId(ObjectId roomId) =>
            await MemberWorker.DeleteMembersByRoomId(roomId);
    }
}