using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class MemberWorker
    {
        #region CheckMemberInRoom
        public static async Task<bool> CheckMemberInRoomByLogin(ObjectId roomId, string login)
        {
            var roomIdFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);
            var loginFilter = Builders<MembersDB_MemberDTO>.Filter.Where(member => member.Login.ToLower() == login);
            var filter = roomIdFilter & loginFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member != null;
        }

        public static async Task<bool> CheckMemberInRoomByEmail(ObjectId roomId, string email)
        {
            var roomIdFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);
            var emailFilter = Builders<MembersDB_MemberDTO>.Filter.Where(member => member.Email.ToLower() == email);
            var filter = roomIdFilter & emailFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member != null;
        }
        #endregion

        #region IO
        public static async Task<MembersDB_MemberDTO[]?> GetMembersByRoomId(ObjectId roomId)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);
            var members = await MongoController.MemberCollection.Find(filter).ToListAsync();

            return members.ToArray();
        }

        public static async Task<MembersDB_MemberDTO?> GetMemberById(ObjectId memberId)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("Id", memberId);
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member;
        }

        public static async Task<MembersDB_MemberDTO[]> GetMembersByIds(ObjectId[] memberIds)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.In("Id", memberIds);
            var members = await MongoController.MemberCollection.Find(filter).ToListAsync();
            return members.ToArray();
        }

        public static async Task<MembersDB_MemberDTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash)
        {
            var roomIdFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);

            var loginFilter = Builders<MembersDB_MemberDTO>.Filter.Where(member =>
                member.Login.ToLower() == login || member.Email.ToLower() == login );

            var passwordFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("PasswordHash", passwordHash);


            var filter = roomIdFilter & loginFilter & passwordFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member;
        }

        public static async Task<MembersDB_MemberDTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash)
        {
            var roomIdFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);
            var fieldsHashFilter = Builders<MembersDB_MemberDTO>.Filter.Eq("FieldsHash", fieldsHash);
            var filter = roomIdFilter & fieldsHashFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member;
        }

        public static async Task<ObjectId> InsertMember(MembersDB_MemberDTO member)
        {
            await MongoController.MemberCollection.InsertOneAsync(member);

            return member.Id;
        }

        public static async Task<ObjectId> Insert(this MembersDB_MemberDTO member) => await InsertMember(member);

        public static async Task<ObjectId[]> InsertMembers(MembersDB_MemberDTO[] members)
        {
            await MongoController.MemberCollection.InsertManyAsync(members);
            return members.Select(members => members.Id).ToArray();
        }

        public static async Task<ObjectId[]> Insert(this MembersDB_MemberDTO[] members) => await InsertMembers(members);

        public static async Task<bool> ReplaceMember(MembersDB_MemberDTO updatedMember)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("Id", updatedMember.Id);
            
            return (await MongoController.MemberCollection.ReplaceOneAsync(filter, updatedMember)).MatchedCount == 1;
        }

        public static async Task<bool> DeleteMember(ObjectId memberId)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("Id", memberId);
            var isDeleteSuccessful = (await MongoController.MemberCollection.DeleteOneAsync(filter)).DeletedCount == 1;
            
            return isDeleteSuccessful;
        }
        public static async Task<bool> DeleteFromDB(this MembersDB_MemberDTO member) => await DeleteMember(member.Id);

        public static async Task<long> DeleteMembersByRoomId(ObjectId roomId)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("RoomId", roomId);
            var deletedCount = (await MongoController.MemberCollection.DeleteManyAsync(filter)).DeletedCount;

            return deletedCount;
        }
        #endregion

        #region Properties
        public static async Task<bool> SetProperty(ObjectId memberId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_MemberDTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId memberId, Dictionary<string, object> properties)
        {
            var filter = Builders<MembersDB_MemberDTO>.Filter.Eq("Id", memberId);
            var updates = new List<UpdateDefinition<MembersDB_MemberDTO>>();
            var update = Builders<MembersDB_MemberDTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
