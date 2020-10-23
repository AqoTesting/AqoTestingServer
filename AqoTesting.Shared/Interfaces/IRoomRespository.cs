using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRespository
    {
        Task<Room> GetRoomById(ObjectId roomId);
        Task<Room> GetRoomByDomain(string domain);
        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);
        Task<ObjectId> InsertRoom(Room newRoom);
        Task DeleteRoomById(ObjectId roomId);
    }
}
