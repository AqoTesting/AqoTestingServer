using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        Room _roomById;
        Room _roomByDomain;

        public async Task<Room> GetRoomById(ObjectId roomId) =>
            await Task.Run(() =>
            {
                if (_roomById == null) _roomById = RoomWorker.GetRoomById(roomId);
                return _roomById;
            });

        public async Task<Room> GetRoomByDomain(string domain) =>
            await Task.Run(() =>
            {
                if (_roomByDomain == null) _roomByDomain = RoomWorker.GetRoomByDomain(domain);
                return _roomByDomain;
            });

        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId) =>
            await Task.Run(() => UserWorker.GetUserRooms(ownerId));

        public async Task<ObjectId> InsertRoom(Room newRoom) =>
            await Task.Run(() => RoomWorker.InsertRoom(newRoom));

        public async Task ReplaceRoom(ObjectId roomId, Room update) =>
            await Task.Run(() => RoomWorker.ReplaceRoom(roomId, update));

        public async Task<bool> RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId) =>
            await Task.Run(() => RoomWorker.RemoveMemberFromRoomById(roomId, memberId));

        public async Task<bool> DeleteRoomById(ObjectId roomId) =>
            await Task.Run(() => RoomWorker.DeleteRoomById(roomId));
    }
}