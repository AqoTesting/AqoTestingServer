using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRespository
    {
        Task<Room> GetRoomById(ObjectId roomId);
        Task<Room> GetRoomByDomain(string domain);
        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);
        Task<ObjectId> InsertRoom(Room newRoom);
        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
