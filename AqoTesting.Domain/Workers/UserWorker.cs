using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace AqoTesting.Domain.Workers
{
    public static class UserWorker
    {
        public static Room[] GetUserRooms(ObjectId UserId)
        {
            var filter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var rooms = MongoController.mainDatabase.GetCollection<Room>("rooms").Find(filter).ToList();
            return rooms.ToArray();
        }

        public static ObjectId[] GetUserRoomsId(ObjectId UserId)
        {
            var filter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var rooms = MongoController.mainDatabase.GetCollection<Room>("rooms").Find(filter).Project<Room>("{ _id:1}").ToList();
            var ids = new List<ObjectId>();
            foreach (var room in rooms)
                ids.Add(room.Id);

            return ids.ToArray();
        }

        public static bool IsUserOwner(ObjectId UserId, ObjectId RoomId)
        {
            var idFilter = Builders<Room>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var filter = idFilter & ownerFilter;
            var isOwner = MongoController.mainDatabase.GetCollection<Room>("rooms").Find(filter).CountDocuments() == 1;
            return isOwner;
        }
    }
}
