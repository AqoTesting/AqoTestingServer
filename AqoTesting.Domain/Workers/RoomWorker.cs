using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
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

        public static async Task<RoomsDB_Room_DTO[]> GetRoomsByUserId(ObjectId userId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("UserId", userId);
            var rooms = await MongoController.RoomCollection.Find(filter).ToListAsync();

            return rooms.ToArray();
        }

        public static async Task<ObjectId> InsertRoom(RoomsDB_Room_DTO room)
        {
            await MongoController.RoomCollection.InsertOneAsync(room);
            return room.Id;
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

        #region Properties
        public static async Task<bool> SetProperty(ObjectId roomId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var update = Builders<RoomsDB_Room_DTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId roomId, Dictionary<string, object> properties)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", roomId);
            var updates = new List<UpdateDefinition<RoomsDB_Room_DTO>>();
            var update = Builders<RoomsDB_Room_DTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.RoomCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
