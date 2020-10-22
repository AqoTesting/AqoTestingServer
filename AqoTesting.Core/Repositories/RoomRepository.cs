using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories {
    public class RoomRespository : IRoomRespository {
        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId) {
            return await Task.Run(() => MongoIOController.GetRoomsByOwnerId(ownerId));
        }

        public async Task<Room> GetRoomByDomain(string domain) {
            return await Task.Run(() => MongoIOController.GetRoomByDomain(domain));
        }

        public async Task<ObjectId> InsertRoom(Room newRoom) {
            return await Task.Run(() => MongoIOController.InsertRoom(newRoom));
        }
    }
}
