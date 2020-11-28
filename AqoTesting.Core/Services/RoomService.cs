using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.Common.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class RoomService : ServiceBase, IRoomService
    {
        IUserRepository _userRepository;
        IRoomRepository _roomRepository;
        IWorkContext _workContext;

        public RoomService(IUserRepository userRepository, IRoomRepository roomRespository, IWorkContext workContext)
        {
            _userRepository = userRepository;
            _roomRepository = roomRespository;
            _workContext = workContext;
        }

        #region User API
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(string roomDomain)
        {
            var roomExists = await _roomRepository.GetRoomByDomain(roomDomain);

            var booleanResponse_DTO = new CommonAPI_BooleanResponse_DTO { BooleanValue = roomExists != null };

            return (OperationErrorMessages.NoError, booleanResponse_DTO);
        }
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(CommonAPI_RoomDomain_DTO roomDomainDTO) =>
            await this.CheckRoomDomainExists(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var getRoomDTO = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(CommonAPI_RoomId_DTO roomIdDTO) =>
            await UserAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var getRoomDTO = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(CommonAPI_RoomDomain_DTO roomDomainDTO) =>
            await this.UserAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(ObjectId userId)
        {
            var userExists = await _userRepository.GetUserById(userId);
            if(userExists == null)
                return (OperationErrorMessages.UserNotFound, null);

            var rooms = await _roomRepository.GetRoomsByUserId(userId);
            var getRoomsItemDTOs = Mapper.Map<UserAPI_GetRoomsItem_DTO[]>(rooms);

            return(OperationErrorMessages.NoError,  getRoomsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(CommonAPI_UserId_DTO userIdDTO) =>
            await UserAPI_GetRoomsByUserId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<(OperationErrorMessages, object)> UserAPI_CreateRoom(UserAPI_PostRoom_DTO postRoomDto)
        {
            var domainTaken = _roomRepository.GetRoomByDomain(postRoomDto.Domain);
            if(domainTaken != null)
                return (OperationErrorMessages.DomainAlreadyTaken, null);

            var newRoom = Mapper.Map<RoomsDB_Room_DTO>(postRoomDto);
            newRoom.UserId = _workContext.UserId;

            var newRoomId = await _roomRepository.InsertRoom(newRoom);
            var newRoomIdDTO = new CommonAPI_RoomId_DTO { RoomId = newRoomId.ToString() };

            return (OperationErrorMessages.NoError, newRoomIdDTO);
        }

        public async Task<OperationErrorMessages> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoom_DTO postRoomDTO)
        {
            var outdatedRoom = await _roomRepository.GetRoomById(roomId);

            if(outdatedRoom.Domain != postRoomDTO.Domain)
            {
                var alreadyTaken = await _roomRepository.GetRoomByDomain(postRoomDTO.Domain);

                if(alreadyTaken != null)
                    return OperationErrorMessages.DomainAlreadyTaken;
            }


            var updatedRoom = Mapper.Map<RoomsDB_Room_DTO>(postRoomDTO);

            updatedRoom.Id = outdatedRoom.Id;
            updatedRoom.UserId = outdatedRoom.UserId;

            await _roomRepository.ReplaceRoom(updatedRoom);

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_EditRoom(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates) =>
            await UserAPI_EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

        public async Task<OperationErrorMessages> UserAPI_DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if(deleted)
                return OperationErrorMessages.RoomNotFound;

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_DeleteRoomById(CommonAPI_RoomId_DTO roomIdDTO) =>
            await UserAPI_DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(CommonAPI_RoomId_DTO roomIdDTO) =>
            await this.MemberAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(CommonAPI_RoomDomain_DTO roomDomainDTO) =>
            await this.MemberAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);
        #endregion
    }
}