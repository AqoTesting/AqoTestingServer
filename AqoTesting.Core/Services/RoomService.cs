using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class RoomService : ServiceBase, IRoomService
    {
        IRoomRepository _roomRepository;
        IWorkContext _workContext;

        public RoomService(IRoomRepository roomRespository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _workContext = workContext;
        }

        public async Task<GetRoomDTO> GetRoomById(RoomIdDTO roomIdDTO)
        {
            var roomId = ObjectId.Parse(roomIdDTO.RoomId);

            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                throw new ResultException(OperationErrorMessages.RoomNotFound);

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
            var roomId = ObjectId.Parse(roomIdDTO.RoomId);

            var outdatedRoom = await this.GetRoomById(roomIdDTO);

            var somethingChanged = false;

            if (roomUpdates.Name != null && outdatedRoom.Name != roomUpdates.Name)
            {
                outdatedRoom.Name = roomUpdates.Name;
                await _roomRepository.SetRoomName(roomId, roomUpdates.Name);
                somethingChanged = true;
            }
            if (roomUpdates.Domain != null && outdatedRoom.Domain != roomUpdates.Domain)
            {
                outdatedRoom.Domain = roomUpdates.Domain;
                await _roomRepository.SetRoomDomain(roomId, roomUpdates.Domain);
                somethingChanged = true;
            }
            if (roomUpdates.RequestedFields != null && outdatedRoom.RequestedFields != roomUpdates.RequestedFields)
            {
                outdatedRoom.RequestedFields = roomUpdates.RequestedFields;
                await _roomRepository.SetRoomRequestedFields(roomId, roomUpdates.RequestedFields);
                somethingChanged = true;
            }
            if (roomUpdates.IsDataRequired != null && outdatedRoom.IsDataRequired != roomUpdates.IsDataRequired)
            {
                outdatedRoom.IsDataRequired = roomUpdates.IsDataRequired.Value;
                await _roomRepository.SetRoomIsDataRequired(roomId, roomUpdates.IsDataRequired.Value);
                somethingChanged = true;
            }
            if (roomUpdates.IsActive != null && outdatedRoom.IsActive != roomUpdates.IsActive)
            {
                outdatedRoom.IsActive = roomUpdates.IsActive.Value;
                await _roomRepository.SetRoomIsActive(roomId, roomUpdates.IsActive.Value);
                somethingChanged = true;
            }

            if (!somethingChanged)
                throw new ResultException(OperationErrorMessages.NothingChanged);

            return outdatedRoom;
        }

        public async Task RemoveMemberFromRoomByTokenById(RoomIdDTO roomIdDTO, MemberTokenDTO memberIdDTO)
        {
            var roomId = ObjectId.Parse(roomIdDTO.RoomId);
            var memberId = memberIdDTO.MemberToken;

            var removed = await _roomRepository.RemoveMemberFromRoomByTokenById(roomId, memberId);

            if (!removed)
                throw new ResultException(OperationErrorMessages.TestNotFound);
        }

        public async Task DeleteRoomById(RoomIdDTO roomIdDTO)
        {
            var roomId = ObjectId.Parse(roomIdDTO.RoomId);

            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if (!deleted)
                throw new ResultException(OperationErrorMessages.RoomNotFound);
        }
    }
}