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
                _roomById = await _cache.Get<RoomsDB_Room_DTO>($"Room:{roomId}", () => RoomWorker.GetRoomById(roomId)) :
                _roomById;

        public async Task<RoomsDB_Room_DTO> GetRoomByDomain(string domain) =>
            await Task.Run(() =>
                _roomByDomain == null ?
                    _roomByDomain = RoomWorker.GetRoomByDomain(domain) :
                    _roomByDomain);

        public async Task<RoomsDB_Room_DTO[]> GetRoomsByOwnerId(ObjectId ownerId) =>
            await Task.Run(() => UserWorker.GetUserRooms(ownerId));

        public async Task<ObjectId> InsertRoom(RoomsDB_Room_DTO newRoom) =>
            await Task.Run(() => RoomWorker.InsertRoom(newRoom));

        public async Task ReplaceRoom(RoomsDB_Room_DTO update) =>
            await Task.Run(() => RoomWorker.ReplaceRoom(update));

        public async Task<bool> RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId) =>
            await Task.Run(() => RoomWorker.RemoveMemberFromRoomById(roomId, memberId));

        public async Task<bool> DeleteRoomById(ObjectId roomId) =>
            await Task.Run(() => RoomWorker.DeleteRoomById(roomId));
    }
}