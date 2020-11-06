using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Users;
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

        public async Task<GetRoomDTO> GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                throw new ResultException(OperationErrorMessages.RoomNotFound);

            return Mapper.Map<GetRoomDTO>(room);
        }
        public async Task<GetRoomDTO> GetRoomById(RoomIdDTO roomIdDTO) =>
            await GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<Room> GetRoomByDomain(string domain) =>
            await _roomRepository.GetRoomByDomain(domain);

        public async Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId)
        {
            var rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);

            var responseRooms = Mapper.Map<GetRoomsItemDTO[]>(rooms);

            return responseRooms;
        }
        public async Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(UserIdDTO userIdDTO) =>
            await GetRoomsByOwnerId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<string> InsertRoom(CreateRoomDTO newRoomDto)
        {
            var newRoom = Mapper.Map<Room>(newRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            return (await _roomRepository.InsertRoom(newRoom)).ToString();
        }

        public async Task<GetRoomDTO> EditRoom(ObjectId roomId, EditRoomDTO roomUpdates)
        {
            var outdatedRoom = await this.GetRoomById(roomId);

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
            if (roomUpdates.Description != null && outdatedRoom.Description != roomUpdates.Description)
            {
                outdatedRoom.Description = roomUpdates.Description;
                await _roomRepository.SetRoomDomain(roomId, roomUpdates.Description);
                somethingChanged = true;
            }
            if (roomUpdates.Fields != null && outdatedRoom.Fields != roomUpdates.Fields)
            {
                outdatedRoom.Fields = roomUpdates.Fields;
                await _roomRepository.SetRoomFields(roomId, Mapper.Map<RoomField[]>(roomUpdates.Fields)); // костыль
                somethingChanged = true;
            }
            if (roomUpdates.IsActive != null && outdatedRoom.IsActive != roomUpdates.IsActive)
            {
                outdatedRoom.IsActive = roomUpdates.IsActive.Value;
                await _roomRepository.SetRoomIsActive(roomId, roomUpdates.IsActive.Value);
                somethingChanged = true;
            }
            if (roomUpdates.IsApproveManually != null && outdatedRoom.IsApproveManually != roomUpdates.IsApproveManually)
            {
                outdatedRoom.IsApproveManually = roomUpdates.IsApproveManually.Value;
                await _roomRepository.SetRoomIsActive(roomId, roomUpdates.IsApproveManually.Value);
                somethingChanged = true;
            }
            if (roomUpdates.IsRegistrationEnabled != null && outdatedRoom.IsRegistrationEnabled != roomUpdates.IsRegistrationEnabled)
            {
                outdatedRoom.IsRegistrationEnabled = roomUpdates.IsRegistrationEnabled.Value;
                await _roomRepository.SetRoomIsActive(roomId, roomUpdates.IsRegistrationEnabled.Value);
                somethingChanged = true;
            }

            if (!somethingChanged)
                throw new ResultException(OperationErrorMessages.NothingChanged);

            return outdatedRoom;
        }
        public async Task<GetRoomDTO> EditRoom(RoomIdDTO roomIdDTO, EditRoomDTO roomUpdates) =>
            await EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

        public async Task<GetEditRoomDTO> GetEditRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                throw new ResultException(OperationErrorMessages.RoomNotFound);

            return Mapper.Map<GetEditRoomDTO>(room);
        }

        public async Task<GetEditRoomDTO> GetEditRoomById(RoomIdDTO roomIdDTO) =>
            await GetEditRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task RemoveMemberFromRoomByTokenById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomByIdById(roomId, memberId);

            if (!removed)
                throw new ResultException(OperationErrorMessages.TestNotFound);
        }
        public async Task RemoveMemberFromRoomByIdById(ObjectId roomId, MemberIdDTO memberIdDTO) =>
            await RemoveMemberFromRoomByTokenById(roomId, ObjectId.Parse(memberIdDTO.MemberId));
        public async Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, ObjectId memberId) =>
            await RemoveMemberFromRoomByTokenById(ObjectId.Parse(roomIdDTO.RoomId), memberId);
        public async Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, MemberIdDTO memberIdDTO) =>
            await RemoveMemberFromRoomByTokenById(ObjectId.Parse(roomIdDTO.RoomId), ObjectId.Parse(memberIdDTO.MemberId));

        public async Task DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if (!deleted)
                throw new ResultException(OperationErrorMessages.RoomNotFound);
        }
        public async Task DeleteRoomById(RoomIdDTO roomIdDTO) =>
            await DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomByIdById(roomId, memberId);

            if(!removed)
                throw new ResultException(OperationErrorMessages.MemberNotFound);
        }
    }
}