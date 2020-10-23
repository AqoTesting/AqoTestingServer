using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        Task<Room> GetRoomById(ObjectId roomId);

        Task<Room> GetRoomByDomain(string domain);

        Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId);

        Task<string> InsertRoom(CreateRoomDTO newRoomDto, string ownerId);

        Task DeleteRoomById(string roomId);
    }
}
