using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomRepository
    {
        Task<RoomsDB_Room_DTO> GetRoomById(ObjectId roomId);

        Task<RoomsDB_Room_DTO> GetRoomByDomain(string domain);

        Task<RoomsDB_Room_DTO[]> GetRoomsByOwnerId(ObjectId ownerId);

        Task<ObjectId> InsertRoom(RoomsDB_Room_DTO newRoom);

        Task ReplaceRoom(RoomsDB_Room_DTO update);

        Task<bool> DeleteRoomById(ObjectId roomId);
    }
}
