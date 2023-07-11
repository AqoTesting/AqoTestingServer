using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public RoomRepository()
        {
        }

        public async Task<RoomsDB_RoomDTO> GetRoomById(ObjectId roomId) =>
            await RoomWorker.GetRoomById(roomId);

        public async Task<RoomsDB_RoomDTO> GetRoomByDomain(string roomDomain) =>
            await RoomWorker.GetRoomByDomain(roomDomain);

        public async Task<RoomsDB_RoomDTO[]> GetRoomsByUserId(ObjectId userId) =>
            await RoomWorker.GetRoomsByUserId(userId);


        public async Task<ObjectId> InsertRoom(RoomsDB_RoomDTO newRoom) =>
            await RoomWorker.InsertRoom(newRoom);

        public async Task ReplaceRoom(RoomsDB_RoomDTO update) =>
            await RoomWorker.ReplaceRoom(update);


        public async Task SetTags(ObjectId roomId, RoomsDB_TagDTO[] newValue) =>
            await RoomWorker.SetProperty(roomId, "Tags", newValue);

        public async Task<bool> SetProperty(ObjectId roomId, string propertyName, object newPropertyValue) =>
            await RoomWorker.SetProperty(roomId, propertyName, newPropertyValue);

        public async Task<bool> SetProperties(ObjectId roomId, Dictionary<string, object> properties) =>
            await RoomWorker.SetProperties(roomId, properties);


        public async Task<bool> DeleteRoomById(ObjectId roomId) =>
            await RoomWorker.DeleteRoomById(roomId);
    }
}