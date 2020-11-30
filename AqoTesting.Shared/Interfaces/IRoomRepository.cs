using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRepository
    {
        Task<RoomsDB_RoomDTO> GetRoomById(ObjectId roomId);

        Task<RoomsDB_RoomDTO> GetRoomByDomain(string domain);

        Task<RoomsDB_RoomDTO[]> GetRoomsByUserId(ObjectId userId);

        Task<ObjectId> InsertRoom(RoomsDB_RoomDTO newRoom);

        Task ReplaceRoom(RoomsDB_RoomDTO update);

        Task<bool> SetProperty(ObjectId roomId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId roomId, Dictionary<string, object> properties);

        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
