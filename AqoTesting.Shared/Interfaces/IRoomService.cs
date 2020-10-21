using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.DTOs.BD.Users;
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
