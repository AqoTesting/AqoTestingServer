using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces {
    public interface IRoomService {
        Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId);
        Task<Room> GetRoomByDomain(string domain);

        Task<ObjectId> InsertRoom(CreateRoomDTO newRoomDto, ObjectId ownerId);
    }
}
