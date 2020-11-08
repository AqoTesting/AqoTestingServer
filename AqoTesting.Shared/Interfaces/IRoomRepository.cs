using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomById(ObjectId roomId);

        Task<Room> GetRoomByDomain(string domain);

        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);

        Task<ObjectId> InsertRoom(Room newRoom);

        Task ReplaceRoom(ObjectId roomId, Room update);

        Task<bool> RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId);

        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
