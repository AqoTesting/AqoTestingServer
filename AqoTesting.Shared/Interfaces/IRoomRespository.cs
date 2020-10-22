using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Rooms;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces {
    public interface IRoomRespository {
        Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId);
        Task<Room> GetRoomByDomain(string domain);

        Task<ObjectId> InsertRoom(Room newRoom);
    }
}
