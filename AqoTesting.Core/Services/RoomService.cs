using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.DB;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Services {
    public class RoomService : ServiceBase, IRoomService {
        IRoomRespository _roomRepository;

        public RoomService(IRoomRespository roomRespository) {
            _roomRepository = roomRespository;
        }

        public async Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId) {
            Room[] rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);

            GetRoomsItemDTO[] responseRooms = new GetRoomsItemDTO[rooms.Length];

            for (var i = 0; i < rooms.Length; i++) {
                var room = rooms[i];

                responseRooms[i] = new GetRoomsItemDTO {
                    Id             = room.Id,
                    Name           = room.Name,
                    Domain         = room.Domain,
                    OwnerId        = room.OwnerId,
                    IsDataRequired = room.IsDataRequired,
                    IsActive       = room.IsActive
                };
            }

            return responseRooms;
        }

        public async Task<Room> GetRoomByDomain(string domain) {
            return await _roomRepository.GetRoomByDomain(domain);
        }

        public async Task<ObjectId> InsertRoom(CreateRoomDTO newRoomDto, ObjectId ownerId) {
            Room newRoom = new Room {
                Name = newRoomDto.Name,
                Domain = newRoomDto.Domain,
                Members = new Member[0],
                TestIds = new ObjectId[0],
                OwnerId = ownerId,
                IsDataRequired = false,
                RequestedFields = new RequestedField[0],
                IsActive = false
            };

            return await _roomRepository.InsertRoom(newRoom);
        }
    }
}