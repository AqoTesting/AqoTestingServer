using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.DTOs.BD.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces {
    public interface IRoomService {
        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);
    }
}
