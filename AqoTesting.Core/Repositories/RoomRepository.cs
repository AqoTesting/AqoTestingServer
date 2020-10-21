using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.DTOs.BD.Users;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories {
    public class RoomRespository : IRoomRespository {
        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId) {
            return await Task.Run(() => MongoIOController.GetRoomsByOwnerId(ownerId));
        }
    }
}
