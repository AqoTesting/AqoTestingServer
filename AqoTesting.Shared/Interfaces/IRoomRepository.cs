using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRepository
    {
        Task<RoomsDB_Room_DTO> GetRoomById(ObjectId roomId);

        Task<RoomsDB_Room_DTO> GetRoomByDomain(string domain);

        Task<RoomsDB_Room_DTO[]> GetRoomsByUserId(ObjectId userId);

        Task<ObjectId> InsertRoom(RoomsDB_Room_DTO newRoom);

        Task ReplaceRoom(RoomsDB_Room_DTO update);

        Task<bool> SetProperty(ObjectId roomId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId roomId, Dictionary<string, object> properties);

        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
