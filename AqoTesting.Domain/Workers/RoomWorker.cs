using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AqoTesting.Domain.Workers
{
    public static class RoomWorker
    {
        #region IO
        public static Room GetRoomById(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = MongoController.RoomCollection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room GetRoomByDomain(string domain)
        {
            var filter = Builders<Room>.Filter.Eq("Domain", domain);
            var room = MongoController.RoomCollection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room[] GetRoomsByOwnerId(ObjectId ownerId)
        {
            var filter = Builders<Room>.Filter.Eq("OwnerId", ownerId);
            var rooms = MongoController.RoomCollection.Find(filter).ToEnumerable();

            return rooms.ToArray();
        }

        public static ObjectId InsertRoom(Room room)
        {
            MongoController.RoomCollection.InsertOne(room);
            return room.Id;
        }

        public static ObjectId[] InsertRooms(Room[] rooms)
        {
            MongoController.RoomCollection.InsertMany(rooms);
            return rooms.Select(room => room.Id).ToArray();
        }

        public static bool DeleteRoomById(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var isDeleteSuccessful = MongoController.RoomCollection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Members
        public static void AddMemberToRoom(ObjectId roomId, ObjectId memberId)
        {
            MemberWorker.SetMemberRoomId(memberId, roomId);
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Members", memberId);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void AddMember(this Room room, ObjectId memberId) => AddMemberToRoom(room.Id, memberId);
        public static void AddMemberToRoom(ObjectId roomId, Member member)
        {
            member.RoomId = roomId;
            MemberWorker.InsertMember(member);
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Members", member.Id);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void AddMember(this Room room, Member member) => AddMemberToRoom(room.Id, member);

        public static bool RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Pull("Members", memberId);
            var isRemovedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;

            return isRemovedSuccessful;
        }
        public static void RemoveMemberById(this Room room, ObjectId memberId) => RemoveMemberFromRoomById(room.Id, memberId);

        public static void RemoveMemberFromRoomByLogin(ObjectId roomId, string memberLogin)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var member = MemberWorker.GetMemberFromRoom(roomId, memberLogin);
            if (member != null)
            {
                var update = Builders<Room>.Update.Pull("Members", member);
                MongoController.RoomCollection.UpdateOne(filter, update);
            }
        }
        public static void RemoveMemberByLogin(this Room room, string memberLogin) => RemoveMemberFromRoomByLogin(room.Id, memberLogin);

        #endregion

        #region Tests
        public static bool AddTestToRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("TestIds", testId);
            var isUpdatedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }
        public static bool AddTestById(this Room room, ObjectId testId) => AddTestToRoomById(room.Id, testId);

        public static bool RemoveTestFromRoomById(ObjectId roomId, ObjectId testId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Pull("TestIds", testId);
            var isUpdatedSuccessful = MongoController.RoomCollection.UpdateOne(filter, update).ModifiedCount == 1;
            return isUpdatedSuccessful;
        }
        public static bool RemoveTestById(this Room room, ObjectId testId) => RemoveTestFromRoomById(room.Id, testId);

        #endregion

        #region Props

        public static void SetRoomName(ObjectId roomId, string newName)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Name", newName);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void SetName(this Room room, string newName) => SetRoomName(room.Id, newName);

        public static void SetRoomDomain(ObjectId roomId, string newDomain)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Domain", newDomain);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void SetDomain(this Room room, string newDomain) => SetRoomDomain(room.Id, newDomain);

        public static void SetRoomRequestedFields(ObjectId roomId, IUserRoomField[] newRequestedFields)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Fields", newRequestedFields);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void SetRequestedFields(this Room room, IUserRoomField[] newRequestedFields) => SetRoomRequestedFields(room.Id, newRequestedFields);

        public static void SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsDataRequired", newIsDataRequired);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void SetIsDataRequired(this Room room, bool newIsDataRequired) => SetRoomIsDataRequired(room.Id, newIsDataRequired);

        public static void SetRoomIsActive(ObjectId roomId, bool newIsActive)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsActive", newIsActive);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void SetIsActive(this Room room, bool newIsActive) => SetRoomIsActive(room.Id, newIsActive);

        #endregion

        #region Fields

        public static RoomField[]? GetRoomFields(ObjectId roomId)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = MongoController.RoomCollection.Find(filter).Project<Room>("{ Fields:1}").SingleOrDefault();

            return room?.Fields;
        }
        public static RoomField[]? GetFields(this Room room) => GetRoomFields(room.Id);

        public static RoomField? GetRoomFieldByName(ObjectId roomId, string name)
        {
            return null;
        }

        public static void AddFieldToRoom(ObjectId roomId, RoomField field)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Fields", field);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void AddField(this Room room, RoomField field) => AddFieldToRoom(room.Id, field);

        public static void RemoveFieldFromRoom(ObjectId roomId, string fieldName)
        {
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var fieldFilter = Builders<RoomField>.Filter.Eq("Name", fieldName);
            var update = Builders<Room>.Update.PullFilter("Fields", fieldFilter);
            MongoController.RoomCollection.UpdateOne(filter, update);
        }
        public static void RemoveField(this Room room, string fieldName) => RemoveFieldFromRoom(room.Id, fieldName);

        #endregion
    }
}
