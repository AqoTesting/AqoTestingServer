using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class RoomService : ServiceBase, IRoomService
    {
        IRoomRespository _roomRepository;
        IWorkContext _workContext;

        public RoomService(IRoomRespository roomRespository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _workContext = workContext;
        }

        public async Task<GetRoomDTO> GetRoomById(string roomId)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(roomId));

            if (room == null) throw new ResultException(OperationErrorMessages.RoomDoesntExists);

            return Mapper.Map<GetRoomDTO>(room);
        }

        public async Task<Room> GetRoomByDomain(string domain)
        {
            return await _roomRepository.GetRoomByDomain(domain);
        }

        public async Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(string ownerId)
        {
            var rooms = await _roomRepository.GetRoomsByOwnerId(ObjectId.Parse(ownerId));

            var responseRooms = Mapper.Map<GetRoomsItemDTO[]>(rooms);

            return responseRooms;
        }

        public async Task<string> InsertRoom(CreateRoomDTO newRoomDto)
        {
            var newRoom = Mapper.Map<Room>(newRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            return (await _roomRepository.InsertRoom(newRoom)).ToString();
        }

        public async Task<bool> DeleteRoomById(string roomId)
        {
            return await _roomRepository.DeleteRoomById(ObjectId.Parse(roomId));
        }
    }
}