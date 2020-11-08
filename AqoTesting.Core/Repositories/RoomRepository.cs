using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        Room _room;

        public async Task<Room> GetRoomById(ObjectId roomId) =>
            await Task.Run(() =>
            {
                if (_room == null) _room = RoomWorker.GetRoomById(roomId);
                return _room;
            });

        public async Task<Room> GetRoomByDomain(string domain) =>
            await Task.Run(() =>
            {
                if (_room == null) _room = RoomWorker.GetRoomByDomain(domain);
                return _room;
            });

        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId) =>
            await Task.Run(() => UserWorker.GetUserRooms(ownerId));

        public async Task<ObjectId> InsertRoom(Room newRoom) =>
            await Task.Run(() => RoomWorker.InsertRoom(newRoom));

        public async Task ReplaceRoom(ObjectId roomId, Room update) =>
            await Task.Run(() => RoomWorker.ReplaceRoom(roomId, update));

        public async Task<bool> RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId) =>
            await Task.Run(() => RoomWorker.RemoveMemberFromRoomByIdById(roomId, memberId));

        public async Task<bool> DeleteRoomById(ObjectId roomId) =>
            await Task.Run(() => RoomWorker.DeleteRoomById(roomId));
    }
}