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
        IRoomRepository _roomRepository;
        IMemberRepository _memberRepository;
        IWorkContext _workContext;

        public RoomService(IRoomRepository roomRespository, IMemberRepository memberRepository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _memberRepository = memberRepository;
            _workContext = workContext;
        }

        #region User API
        public async Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var responseRoom = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return responseRoom;
        }
        public async Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomById(RoomId_DTO roomIdDTO) =>
            await UserAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var responseRoom = Mapper.Map<UserAPI_GetRoom_DTO>(room);

            return responseRoom;
        }
        public async Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomByDomain(UserAPI_RoomDomain_DTO roomDomainDTO) =>
            await this.UserAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<UserAPI_GetRoomsItem_DTO[]> UserAPI_GetRoomsByOwnerId(ObjectId ownerId)
        {
            var rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);
            var responseRooms = Mapper.Map<UserAPI_GetRoomsItem_DTO[]>(rooms);

            return responseRooms;
        }
        public async Task<UserAPI_GetRoomsItem_DTO[]> UserAPI_GetRoomsByOwnerId(UserId_DTO userIdDTO) =>
            await UserAPI_GetRoomsByOwnerId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<string> UserAPI_InsertRoom(UserAPI_PostRoom_DTO postRoomDto)
        {
            var newRoom = Mapper.Map<Room>(postRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            return (await _roomRepository.InsertRoom(newRoom)).ToString();
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

            await _roomRepository.ReplaceRoom(roomId, updatedRoom);

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_EditRoom(RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates) =>
            await UserAPI_EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

        public async Task UserAPI_DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if(!deleted)
                throw new ResultException(OperationErrorMessages.RoomNotFound);
        }
        public async Task UserAPI_DeleteRoomById(RoomId_DTO roomIdDTO) =>
            await UserAPI_DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task UserAPI_RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomById(roomId, memberId);

            if(!removed)
                throw new ResultException(OperationErrorMessages.MemberNotFound);
        }
        #endregion

        #region Member API
        public async Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var responseRoom = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return responseRoom;
        }
        public async Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(RoomId_DTO roomIdDTO) =>
            await this.MemberAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));
        public async Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(string roomId) =>
            await this.MemberAPI_GetRoomById(ObjectId.Parse(roomId));

        public async Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var responseRoom = Mapper.Map<MemberAPI_GetRoom_DTO>(room);

            return responseRoom;
        }
        public async Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomByDomain(MemberAPI_RoomDomain_DTO roomDomainDTO) =>
            await this.MemberAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);
        #endregion
    }
}