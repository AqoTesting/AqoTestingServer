using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AqoTesting.Domain.Workers
{
    public static class RoomWorker
    {
        #region IO
        public static Room GetRoomById(ObjectId roomId)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = collection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room GetRoomByDomain(string domain)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Domain", domain);
            var room = collection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room[] GetRoomsByOwnerId(ObjectId ownerId)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("OwnerId", ownerId);
            var rooms = collection.Find(filter).ToList();

            return rooms.ToArray();
        }

        public static ObjectId InsertRoom(Room room)
        {
            MongoController.GetRoomsCollection().InsertOne(room);
            return room.Id;
        }

        public static ObjectId[] InsertRooms(Room[] rooms)
        {
            MongoController.GetRoomsCollection().InsertMany(rooms);
            return rooms.Select(room => room.Id).ToArray();
        }

        public static bool DeleteRoomById(ObjectId roomId)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var isDeleteSuccessful = collection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region members

        public static void AddMemberToRoom(ObjectId roomId, Member member)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Push("Members", member);
            collection.UpdateOne(filter, update);
        }

        public static void AddMember(this Room room, Member member) => AddMemberToRoom(room.Id, member);

        public static void RemoveMemberFromRoomByToken(ObjectId roomId, string token)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.PullFilter("Members", Builders<Member>.Filter.Eq("Token", token));
            collection.UpdateOne(filter, update);
        }

        public static void RemoveMemberByToken(this Room room, string token) => RemoveMemberFromRoomByToken(room.Id, token);

        public static void RemoveMemberFromRoomByLogin(ObjectId roomId, string login)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.PullFilter("Members", Builders<Member>.Filter.Eq("Login", login));
            collection.UpdateOne(filter, update);
        }

        public static void RemoveMemberByLogin(this Room room, string login) => RemoveMemberFromRoomByLogin(room.Id, login);

        #endregion

        #region props

        public static void SetRoomName(ObjectId roomId, string newName)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Name", newName);
            collection.UpdateOne(filter, update);
        }

        public static void SetName(this Room room, string newName) => SetRoomName(room.Id, newName);

        public static void SetRoomDomain(ObjectId roomId, string newDomain)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("Domain", newDomain);
            collection.UpdateOne(filter, update);
        }

        public static void SetDomain(this Room room, string newDomain) => SetRoomDomain(room.Id, newDomain);

        public static void SetRoomRequestedFields(ObjectId roomId, RequestedFieldDTO[] newRequestedFields)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("RequestedFields", newRequestedFields);
            collection.UpdateOne(filter, update);
        }

        public static void SetRequestedFields(this Room room, RequestedFieldDTO[] newRequestedFields) => SetRoomRequestedFields(room.Id, newRequestedFields);

        public static void SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsDataRequired", newIsDataRequired);
            collection.UpdateOne(filter, update);
        }

        public static void SetIsDataRequired(this Room room, bool newIsDataRequired) => SetRoomIsDataRequired(room.Id, newIsDataRequired);

        public static void SetRoomIsActive(ObjectId roomId, bool newIsActive)
        {
            var collection = MongoController.GetRoomsCollection();
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var update = Builders<Room>.Update.Set("IsActive", newIsActive);
            collection.UpdateOne(filter, update);
        }

        public static void SetIsActive(this Room room, bool newIsActive) => SetRoomIsActive(room.Id, newIsActive);

        #endregion
    }
}
