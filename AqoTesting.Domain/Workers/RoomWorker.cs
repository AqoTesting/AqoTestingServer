using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class RoomWorker
    {
        #region IO
        public static async Task<RoomsDB_Room_DTO> GetRoomById(ObjectId roomId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var room = await MongoController.RoomCollection.Find(filter).SingleOrDefaultAsync();
            return room;
        }

        public static async Task<RoomsDB_Room_DTO> GetRoomByDomain(string domain)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Domain", domain);
            var room = await MongoController.RoomCollection.Find(filter).SingleOrDefaultAsync();
            return room;
        }

        public static async Task<RoomsDB_Room_DTO[]> GetRoomsByOwnerId(ObjectId ownerId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("OwnerId", ownerId);
            var rooms = await MongoController.RoomCollection.Find(filter).ToListAsync();

            return rooms.ToArray();
        }

        public static async Task<ObjectId> InsertRoom(RoomsDB_Room_DTO room)
        {
            await MongoController.RoomCollection.InsertOneAsync(room);
            return room.Id;
        }

        public static async Task<ObjectId[]> InsertRooms(RoomsDB_Room_DTO[] rooms)
        {
            await MongoController.RoomCollection.InsertManyAsync(rooms);
            return rooms.Select(room => room.Id).ToArray();
        }

        public static async Task<bool> ReplaceRoom(RoomsDB_Room_DTO updatedRoom)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", updatedRoom.Id);
            return (await MongoController.RoomCollection.ReplaceOneAsync(filter, updatedRoom)).MatchedCount == 1;
        }

        public static async Task<bool> UpdateInDB(this RoomsDB_Room_DTO room) => await ReplaceRoom(room);
 
        public static async Task<bool> DeleteRoomById(ObjectId roomId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var isDeleteSuccessful = (await MongoController.RoomCollection.DeleteOneAsync(filter)).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Members

        public static async Task<bool> AddMemberToRoom(ObjectId roomId, ObjectId memberId)
        {
            MemberWorker.SetMemberRoomId(memberId, roomId);
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Push("Member", memberId);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> AddNewMemberToRoom(ObjectId roomId, MembersDB_Member_DTO member)
        {
            member.RoomId = roomId;
            MemberWorker.InsertMember(member);
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Push("Member", member.Id);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Pull("Member", memberId);
            var isRemovedSuccessful = (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;

            return isRemovedSuccessful;
        }

        public static async Task<bool> RemoveMemberFromRoomByLogin(ObjectId roomId, string memberLogin)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var member = MemberWorker.GetMemberFromRoom(roomId, memberLogin);
            if(member != null)
            {
                var update = Builders<RoomsDB_Room_DTO>.Update.Pull("Member", member.Id);
                return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
            }
            return false;
        }

        public static async Task<bool> RemoveMemberByLogin(this RoomsDB_Room_DTO room, string memberLogin) => await RemoveMemberFromRoomByLogin(room.Id, memberLogin);

        #endregion

        #region Tests

        public static async Task<bool> AddTestToRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Push("TestIds", testId);
            var isUpdatedSuccessful = (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }

        public static async Task<bool> AddTestById(this RoomsDB_Room_DTO room, ObjectId testId)
        {
            return await AddTestToRoomById(room.Id, testId); ;
        }

        public static async Task<bool> RemoveTestFromRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Pull("TestIds", testId);
            var isUpdatedSuccessful = (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }

        public static async Task<bool> RemoveTestById(this RoomsDB_Room_DTO room, ObjectId testId)
        {
            return await RemoveTestFromRoomById(room.Id, testId);
        }

        #endregion

        #region Props

        public static async Task<bool> SetRoomName(ObjectId roomId, string newName)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set("Name", newName);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetName(this RoomsDB_Room_DTO room, string newName)
        {
            if (await SetRoomName(room.Id, newName) == true)
            {
                room.Name = newName;
                return true;
            }
            return false;
        }

        public static async Task<bool> SetRoomDomain(ObjectId roomId, string newDomain)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set("Domain", newDomain);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetDomain(this RoomsDB_Room_DTO room, string newDomain)
        {
            if (await SetRoomDomain(room.Id, newDomain) == true)
            {
                room.Domain = newDomain;
                return true;
            }
            return false;
        }

        public static async Task<bool> SetRoomFields(ObjectId roomId, RoomsDB_Field_DTO[] newFields)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set("Fields", newFields);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetRequestedFields(this RoomsDB_Room_DTO room, RoomsDB_Field_DTO[] newFields)
        {
            if (await SetRoomFields(room.Id, newFields) == true)
            {
                room.Fields = newFields;
                return true;
            }
            return false;
        }

        public static async Task<bool> SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set("IsDataRequired", newIsDataRequired);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetIsDataRequired(this RoomsDB_Room_DTO room, bool newIsDataRequired)
        {
            if (await SetRoomIsDataRequired(room.Id, newIsDataRequired) == true)
            {
                room.IsDataRequired = newIsDataRequired;
                return true;
            }
            return false;
        }

        public static async Task<bool> SetRoomIsActive(ObjectId roomId, bool newIsActive)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set("IsActive", newIsActive);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetIsActive(this RoomsDB_Room_DTO room, bool newIsActive)
        {
            if (await SetRoomIsActive(room.Id, newIsActive) == true)
            {
                room.IsActive = newIsActive;
                return true;
            }
            return false;
        }

        #endregion

        #region Fields

        public static async Task<RoomsDB_Field_DTO[]?> GetRoomFields(ObjectId roomId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var room = await MongoController.RoomCollection.Find(filter).Project<RoomsDB_Room_DTO>("{ Fields:1}").SingleOrDefaultAsync();

            return room?.Fields;
        }

        public static async Task<RoomsDB_Field_DTO[]?> GetFields(this RoomsDB_Room_DTO room) =>
            await GetRoomFields(room.Id);

        public static async Task<bool> AddFieldToRoom(ObjectId roomId, RoomsDB_Field_DTO field)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Push("Fields", field);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> AddField(this RoomsDB_Room_DTO room, RoomsDB_Field_DTO field)
        {
            if (await AddFieldToRoom(room.Id, field) == true)
            {
                var fieldsList = room.Fields.ToList();
                fieldsList.Add(field);
                room.Fields = fieldsList.ToArray();
                return true;
            }
            return false;
        }

        public static async Task<bool> RemoveFieldFromRoom(ObjectId roomId, string fieldName)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var fieldFilter = Builders<RoomsDB_Field_DTO>.Filter.Eq("Name", fieldName);
            var update = Builders<RoomsDB_Room_DTO>.Update.PullFilter("Fields", fieldFilter);
            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> RemoveField(this RoomsDB_Room_DTO room, string fieldName)
        {
            if (await RemoveFieldFromRoom(room.Id, fieldName) == true)
            {
                var fieldsList = new List<RoomsDB_Field_DTO>();
                foreach (var field in room.Fields)
                    if (field.Name != fieldName)
                        fieldsList.Add(field);
                room.Fields = fieldsList.ToArray();
                return true;
            }
            return false;
        }
        #endregion
    }
}
