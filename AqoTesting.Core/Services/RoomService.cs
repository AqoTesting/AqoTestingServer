using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class RoomService : ServiceBase, IRoomService
    {
        IRoomRespository _roomRepository;

        public RoomService(IRoomRespository roomRespository)
        {
            _roomRepository = roomRespository;
        }

        public async Task<Room> GetRoomById(ObjectId roomId)
        {
            return await _roomRepository.GetRoomById(roomId);
        }

        public async Task<Room> GetRoomByDomain(string domain)
        {
            return await _roomRepository.GetRoomByDomain(domain);
        }

        public async Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId)
        {
            var rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);

            var responseRooms = Mapper.Map<GetRoomsItemDTO[]>(rooms);

            return responseRooms;
        }

        public async Task<string> InsertRoom(CreateRoomDTO newRoomDto, string ownerId)
        {
            var newRoom = Mapper.Map<Room>(newRoomDto);
            newRoom.OwnerId = ObjectId.Parse(ownerId);

            //return await Task.Run(() => new ObjectId().ToString()); // Затычка для дебага
            return (await _roomRepository.InsertRoom(newRoom)).ToString();
        }

        public async Task<bool> DeleteRoomById(string roomId)
        {
            return await _roomRepository.DeleteRoomById(ObjectId.Parse(roomId));
        }
    }
}