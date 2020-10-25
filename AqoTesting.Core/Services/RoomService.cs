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

            if (room == null)
                throw new ResultException(OperationErrorMessages.RoomDoesntExists);

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

        public async Task<GetRoomDTO> EditRoom(RoomIdDTO roomIdDTO, EditRoomDTO roomUpdates)
        {
            var roomId = roomIdDTO.Id;

            var outdatedRoom = await this.GetRoomById(roomId);

            var somethingChanged = false;

            if (roomUpdates.Name != null && outdatedRoom.Name != roomUpdates.Name)
            {
                outdatedRoom.Name = roomUpdates.Name;
                await _roomRepository.SetRoomName(ObjectId.Parse(roomId), roomUpdates.Name);
                somethingChanged = true;
            }
            if (roomUpdates.Domain != null && outdatedRoom.Domain != roomUpdates.Domain)
            {
                outdatedRoom.Domain = roomUpdates.Domain;
                await _roomRepository.SetRoomDomain(ObjectId.Parse(roomId), roomUpdates.Domain);
                somethingChanged = true;
            }
            if (roomUpdates.RequestedFields != null && outdatedRoom.RequestedFields != roomUpdates.RequestedFields)
            {
                outdatedRoom.RequestedFields = roomUpdates.RequestedFields;
                await _roomRepository.SetRoomRequestedFields(ObjectId.Parse(roomId), roomUpdates.RequestedFields);
                somethingChanged = true;
            }
            if (roomUpdates.IsDataRequired != null && outdatedRoom.IsDataRequired != roomUpdates.IsDataRequired)
            {
                outdatedRoom.IsDataRequired = roomUpdates.IsDataRequired.Value;
                await _roomRepository.SetRoomIsDataRequired(ObjectId.Parse(roomId), roomUpdates.IsDataRequired.Value);
                somethingChanged = true;
            }
            if (roomUpdates.IsActive != null && outdatedRoom.IsActive != roomUpdates.IsActive)
            {
                outdatedRoom.IsActive = roomUpdates.IsActive.Value;
                await _roomRepository.SetRoomIsActive(ObjectId.Parse(roomId), roomUpdates.IsActive.Value);
                somethingChanged = true;
            }

            if (!somethingChanged) throw new ResultException(OperationErrorMessages.NothingChanged);

            return outdatedRoom;
        }

        public async Task DeleteRoomById(string roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(ObjectId.Parse(roomId));
            if (!deleted)
                throw new ResultException(OperationErrorMessages.RoomDoesntExists);
        }
    }
}