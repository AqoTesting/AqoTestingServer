using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using AqoTesting.Shared.DTOs.DB.Attempts;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class MemberWorker
    {
        #region GetMemberFromRoom
        public static async Task<MembersDB_Member_DTO?> GetMemberFromRoom(ObjectId roomId, string memberLogin)
        {
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Login", memberLogin);
            var roomidfilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var filter = loginFilter & roomidfilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();
            return member;
        }

        public static async Task<MembersDB_Member_DTO?> GetMemberFromRoom(RoomsDB_Room_DTO room, string memberLogin)
        {
            return await GetMemberFromRoom(room.Id, memberLogin);
        }

        public static async Task<MembersDB_Member_DTO[]?> GetMembersFromRoom(ObjectId roomId)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var members = await MongoController.MemberCollection.Find(filter).ToListAsync();
            return members.ToArray();
        }

        public static async Task<MembersDB_Member_DTO[]?> GetMembersFromRoom(RoomsDB_Room_DTO room)
        {
            return await GetMembersFromRoom(room.Id);
        }

        public static async Task<MembersDB_Member_DTO?> GetMemberById(ObjectId memberId)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();
            return member;
        }

        public static async Task<MembersDB_Member_DTO[]> GetMembersByIds(ObjectId[] memberIds)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.In("Id", memberIds);
            var members = await MongoController.MemberCollection.Find(filter).ToListAsync();
            return members.ToArray();
        }

        public static async Task<MembersDB_Member_DTO> GetMemberByAuthData(ObjectId roomId, string login, byte[] passwordHash)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Email", login) | Builders<MembersDB_Member_DTO>.Filter.Eq("Login", login);
            var passwordFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("PasswordHash", passwordHash);
            var filter = roomIdFilter & loginFilter & passwordFilter;

            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member;
        }

        public static async Task<MembersDB_Member_DTO> GetMemberByFieldsHash(ObjectId roomId, byte[] fieldsHash)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var fieldsHashFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("FieldsHash", fieldsHash);
            var filter = roomIdFilter & fieldsHashFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member;
        }
        #endregion

        #region CheckMemberInRoom

        public static async Task<bool> CheckMemberInRoomByLogin(ObjectId roomId, string login)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var loginFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Login", login);
            var filter = roomIdFilter & loginFilter;
            var member = MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member != null;
        }

        public static async Task<bool> CheckMemberInRoomByEmail(ObjectId roomId, string email)
        {
            var roomIdFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("RoomId", roomId);
            var emailFilter = Builders<MembersDB_Member_DTO>.Filter.Eq("Email", email);
            var filter = roomIdFilter & emailFilter;
            var member = await MongoController.MemberCollection.Find(filter).SingleOrDefaultAsync();

            return member != null;
        }
        #endregion

        #region IO

        public static async Task<ObjectId> InsertMember(MembersDB_Member_DTO member)
        {
            await MongoController.MemberCollection.InsertOneAsync(member);

            return member.Id;
        }

        public static async Task<ObjectId> Insert(this MembersDB_Member_DTO member) => await InsertMember(member);

        public static async Task<ObjectId[]> InsertMembers(MembersDB_Member_DTO[] members)
        {
            await MongoController.MemberCollection.InsertManyAsync(members);
            return members.Select(members => members.Id).ToArray();
        }

        public static async Task<ObjectId[]> Insert(this MembersDB_Member_DTO[] members) => await InsertMembers(members);

        public static async Task<bool> ReplaceMember(MembersDB_Member_DTO updatedMember)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", updatedMember.Id);
            return (await MongoController.MemberCollection.ReplaceOneAsync(filter, updatedMember)).MatchedCount == 1;
        }

        public static async Task<bool> DeleteMember(MembersDB_Member_DTO member)
        {
            RoomWorker.RemoveMemberFromRoomById(member.RoomId, member.Id);
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", member.Id);
            var isDeleteSuccessful = (await MongoController.MemberCollection.DeleteOneAsync(filter)).DeletedCount == 1;

            return isDeleteSuccessful;
        }

        public static async Task<bool> DeleteMember(ObjectId memberId)
        {
            var member = await GetMemberById(memberId);
            if(member != null)
                return await DeleteMember(member);
            return false;
        }

        public static async Task<bool> DeleteFromDB(this MembersDB_Member_DTO member) => await DeleteMember(member);

        #endregion

        #region Common

        public static async Task<AttemptsDB_Attempt_DTO[]?> GetMemberAttempts(ObjectId memberId, ObjectId testId)
        {
            var memberFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var testFilter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("TestId", testId);
            var filter = memberFilter & testFilter;
            var attempts = (await MongoController.AttemptCollection.Find(filter).ToListAsync()).ToArray();
            return attempts;
        }

        public static async Task<AttemptsDB_Attempt_DTO[]?> GetMemberAttempts(this MembersDB_Member_DTO member, ObjectId testId)
        {
            return await GetMemberAttempts(member.Id, testId);
        }

        public static async Task<AttemptsDB_Attempt_DTO[]?> GetMemberAttempts(ObjectId memberId)
        {
            var filter = Builders<AttemptsDB_Attempt_DTO>.Filter.Eq("MemberId", memberId);
            var attempts = (await MongoController.AttemptCollection.Find(filter).ToListAsync()).ToArray();
            return attempts;
        }

        public static async Task<AttemptsDB_Attempt_DTO[]?> GetMemberAttempts(this MembersDB_Member_DTO member)
        {
            return await GetMemberAttempts(member.Id);
        }

        public static async Task<Dictionary<string, string>?> GetMemberFields(ObjectId memberId)
        {
            var member = await GetMemberById(memberId);

            return member?.Fields;
        }

        public static async Task<Dictionary<string, string>?> GetMemberFields(ObjectId roomId, string memberLogin)
        {
            var member = await GetMemberFromRoom(roomId, memberLogin);

            return member?.Fields;
        }

        public static async Task<byte[]?> GetMemberPasswordHash(ObjectId roomId, string memberLogin)
        {
            var member = await GetMemberFromRoom(roomId, memberLogin);

            return member?.PasswordHash;
        }

        public static async Task<byte[]?> GetMemberPasswordHash(ObjectId memberId)
        {
            var member = await GetMemberById(memberId);

            return member?.PasswordHash;
        }

        public static async Task<bool> SetMemberPasswordHash(ObjectId memberId, byte[] newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("PasswordHash", newValue);
            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetPasswordHash(this MembersDB_Member_DTO member, byte[] newValue)
        {
            if (await SetMemberPasswordHash(member.Id, newValue) == true)
            {
                member.PasswordHash = newValue;
                return true;
            }
            return false;
        }

        public static async Task<bool> SetMemberRoomId(ObjectId memberId, ObjectId newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("RoomId", newValue);
            var isModifySuccessful = (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;

            return isModifySuccessful;
        }

        public static async Task<bool> SetRoomId(this MembersDB_Member_DTO member, ObjectId newValue)
        {
            var roomIdChanged = await SetMemberRoomId(member.Id, newValue);
            if(roomIdChanged == true)
                member.RoomId = newValue;
            return roomIdChanged;
        }

        public static async Task<bool> SetIsApproved(ObjectId memberId, bool newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("IsApproved", newValue);
            var isModifySuccessful = (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;

            return isModifySuccessful;
        }

        public static async Task<bool> SetIsApproved(this MembersDB_Member_DTO member, bool newValue)
        {
            var dbUpdateSuccessful = await SetIsApproved(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.IsApproved = newValue;
            return dbUpdateSuccessful;
        }

        public static async Task<bool> SetIsRegistered(ObjectId memberId, bool newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("IsRegistered", newValue);
            var isModifySuccessful = (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;

            return isModifySuccessful;
        }

        public static async Task<bool> SetIsRegistered(this MembersDB_Member_DTO member, bool newValue)
        {
            var dbUpdateSuccessful = await SetIsRegistered(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.IsRegistered = newValue;
            return dbUpdateSuccessful;
        }

        public static async Task<bool> SetMemberFields(ObjectId memberId, BsonDocument newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("Fields", newValue);
            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetMemberFieldsHash(ObjectId memberId, byte[] newValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set("FieldsHash", newValue);
            var isModifySuccessful = (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;

            return isModifySuccessful;
        }

        public static async Task<bool> SetFieldsHash(this MembersDB_Member_DTO member, byte[] newValue)
        {
            var dbUpdateSuccessful = await SetMemberFieldsHash(member.Id, newValue);
            if(dbUpdateSuccessful == true)
                member.FieldsHash = newValue;
            return dbUpdateSuccessful;
        }

        public static async Task<bool> SetProperty(ObjectId memberId, string propName, object propValue)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var update = Builders<MembersDB_Member_DTO>.Update.Set(propName, propValue);
            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId memberId, Dictionary<string, object> properties)
        {
            var filter = Builders<MembersDB_Member_DTO>.Filter.Eq("Id", memberId);
            var updates = new List<UpdateDefinition<MembersDB_Member_DTO>>();
            var update = Builders<MembersDB_Member_DTO>.Update;
            foreach (KeyValuePair<string, object> kvp in properties)
            {
                updates.Add(update.Set(kvp.Key, kvp.Value));
            }
            return (await MongoController.MemberCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
