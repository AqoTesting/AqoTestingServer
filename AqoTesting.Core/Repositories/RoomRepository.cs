using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
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
                if(_room == null) _room = RoomWorker.GetRoomById(roomId);
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

        public async Task SetRoomName(ObjectId roomId, string newName) =>
            await Task.Run(() => RoomWorker.SetRoomName(roomId, newName));

        public async Task SetRoomDomain(ObjectId roomId, string newDomain) =>
            await Task.Run(() => RoomWorker.SetRoomDomain(roomId, newDomain));

        public async Task SetRoomRequestedFields(ObjectId roomId, RequestedFieldDTO[] newRequestedFields) =>
            await Task.Run(() => RoomWorker.SetRoomRequestedFields(roomId, newRequestedFields));

        public async Task SetRoomIsDataRequired(ObjectId roomId, bool newIsDataRequired) =>
            await Task.Run(() => RoomWorker.SetRoomIsDataRequired(roomId, newIsDataRequired));

        public async Task SetRoomIsActive(ObjectId roomId, bool newIsActive) =>
            await Task.Run(() => RoomWorker.SetRoomIsActive(roomId, newIsActive));

        public async Task<bool> DeleteRoomById(ObjectId roomId) =>
            await Task.Run(() => RoomWorker.DeleteRoomById(roomId));
    }
}
