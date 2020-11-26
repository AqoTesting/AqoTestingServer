using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        RoomsDB_Room_DTO _roomById;
        RoomsDB_Room_DTO _roomByDomain;

        ICacheRepository _cache;
        public RoomRepository(ICacheRepository cache)
        {
            _cache = cache;
        }

        public async Task<RoomsDB_Room_DTO> GetRoomById(ObjectId roomId) =>
            _roomById == null ?
                _roomById = await _cache.Get<RoomsDB_Room_DTO>($"Room:{roomId}", async () => await RoomWorker.GetRoomById(roomId)) :
                _roomById;

        public async Task<RoomsDB_Room_DTO> GetRoomByDomain(string domain) =>
            _roomByDomain == null ?
                _roomByDomain = await RoomWorker.GetRoomByDomain(domain) :
                _roomByDomain;

        public async Task<RoomsDB_Room_DTO[]> GetRoomsByOwnerId(ObjectId ownerId) =>
            await UserWorker.GetUserRooms(ownerId);

        public async Task<ObjectId> InsertRoom(RoomsDB_Room_DTO newRoom) =>
            await RoomWorker.InsertRoom(newRoom);

        public async Task ReplaceRoom(RoomsDB_Room_DTO update)
        {
            await RoomWorker.ReplaceRoom(update);
            await _cache.Del($"Room:{update.Id}");
        }

        public async Task<bool> DeleteRoomById(ObjectId roomId)
        {
            var response = await RoomWorker.DeleteRoomById(roomId);
            await _cache.Del($"Room:{roomId}");

            return response;
        }
    }
}