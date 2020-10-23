using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRespository : IRoomRespository
    {
        Room _room;

        public async Task<Room> GetRoomById(ObjectId roomId)
        {
            return await Task.Run(() =>
            {
                if(_room == null) _room = MongoIOController.GetRoomById(roomId);
                return _room;
            });
        }

        public async Task<Room> GetRoomByDomain(string domain)
        {
            return await Task.Run(() =>
            {
                if (_room == null) _room = MongoIOController.GetRoomByDomain(domain);
                return _room;
            });
        }
        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId)
        {
            return await Task.Run(() => UserWorker.GetUserRooms(ownerId));
        }

        public async Task<ObjectId> InsertRoom(Room newRoom)
        {
            return await Task.Run(() => MongoIOController.InsertRoom(newRoom));
        }

        public async Task<bool> DeleteRoomById(ObjectId roomId)
        {
            return await Task.Run(() => MongoIOController.DeleteRoomById(roomId));
        }
    }
}
