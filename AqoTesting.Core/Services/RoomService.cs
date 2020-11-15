using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
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
        IUserRepository _userRepository;
        IRoomRepository _roomRepository;
        IMemberRepository _memberRepository;
        IWorkContext _workContext;

        public RoomService(IUserRepository userRepository, IRoomRepository roomRespository, IMemberRepository memberRepository, IWorkContext workContext)
        {
            _userRepository = userRepository;
            _roomRepository = roomRespository;
            _memberRepository = memberRepository;
            _workContext = workContext;
        }

        #region User API
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(string roomDomain)
        {
            var roomExists = await _roomRepository.GetRoomByDomain(roomDomain);

            var booleanResponse_DTO = new BooleanResponse_DTO { BooleanValue = roomExists != null };

            return (OperationErrorMessages.NoError, booleanResponse_DTO);
        }
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(RoomDomain_DTO roomDomainDTO) =>
            await this.CheckRoomDomainExists(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(RoomId_DTO roomIdDTO) =>
            await UserAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(RoomDomain_DTO roomDomainDTO) =>
            await this.UserAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByOwnerId(ObjectId ownerId)
        {
            var userExists = await _userRepository.GetUserById(ownerId);
            if(userExists == null)
                return (OperationErrorMessages.UserNotFound, null);

            var rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);
            var getRoomsItemDTOs = Mapper.Map<UserAPI_GetRoomsItem_DTO[]>(rooms);

            return(OperationErrorMessages.NoError,  getRoomsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByOwnerId(UserId_DTO userIdDTO) =>
            await UserAPI_GetRoomsByOwnerId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<(OperationErrorMessages, object)> UserAPI_InsertRoom(UserAPI_PostRoom_DTO postRoomDto)
        {
            var domainTaken = _roomRepository.GetRoomByDomain(postRoomDto.Domain);
            if(domainTaken != null)
                return (OperationErrorMessages.DomainAlreadyTaken, null);

            var newRoom = Mapper.Map<Room>(postRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            var newRoomId = await _roomRepository.InsertRoom(newRoom);
            var newRoomIdDTO = new RoomId_DTO { RoomId = newRoomId.ToString() };

            return (OperationErrorMessages.NoError, newRoomIdDTO);
        }

        public async Task<OperationErrorMessages> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoom_DTO postRoomDTO)
        {
            var outdatedRoom = await _roomRepository.GetRoomById(roomId);

            if(outdatedRoom == null)
                return OperationErrorMessages.RoomNotFound;

            if(outdatedRoom.Domain != postRoomDTO.Domain)
            {
                var alreadyTaken = await _roomRepository.GetRoomByDomain(postRoomDTO.Domain);

                if(alreadyTaken != null)
                    return OperationErrorMessages.DomainAlreadyTaken;
            }


            var updatedRoom = Mapper.Map<Room>(postRoomDTO);

            updatedRoom.Id = outdatedRoom.Id;
            updatedRoom.OwnerId = outdatedRoom.OwnerId;

            await _roomRepository.ReplaceRoom(updatedRoom);

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_EditRoom(RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates) =>
            await UserAPI_EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

        public async Task<OperationErrorMessages> UserAPI_DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if(deleted)
                return OperationErrorMessages.RoomNotFound;

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_DeleteRoomById(RoomId_DTO roomIdDTO) =>
            await UserAPI_DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<OperationErrorMessages> UserAPI_RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomById(roomId, memberId);

            if(!removed)
                return OperationErrorMessages.MemberNotFound;

            return OperationErrorMessages.NoError;
        }
        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(RoomId_DTO roomIdDTO) =>
            await this.MemberAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(RoomDomain_DTO roomDomainDTO) =>
            await this.MemberAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);
        #endregion
    }
}