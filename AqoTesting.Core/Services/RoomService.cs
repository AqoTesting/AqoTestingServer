using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Services {
    public class RoomService : ServiceBase, IRoomService {
        IRoomRespository _roomRepository;

        public RoomService(IRoomRespository roomRespository) {
            _roomRepository = roomRespository;
        }

        public async Task<Room[]> GetRoomsByOwnerId(ObjectId ownerId) {
            return await _roomRepository.GetRoomsByOwnerId(ownerId);
        }
    }
}