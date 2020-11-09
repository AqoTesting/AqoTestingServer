using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
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

        public async Task<GetUserRoomDTO> GetUserRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var responseRoom = Mapper.Map<GetUserRoomDTO>(room);

            return responseRoom;
        }
        public async Task<GetUserRoomDTO> GetUserRoomById(UserRoomIdDTO roomIdDTO) =>
            await GetUserRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<GetUserRoomDTO> GetUserRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var responseRoom = Mapper.Map<GetUserRoomDTO>(room);

            return responseRoom;
        }
        public async Task<GetUserRoomDTO> GetUserRoomByDomain(UserRoomDomainDTO roomDomainDTO) =>
            await this.GetUserRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<GetMemberRoomDTO> GetMemberRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var responseRoom = Mapper.Map<GetMemberRoomDTO>(room);

            return responseRoom;
        }
        public async Task<GetMemberRoomDTO> GetMemberRoomById(MemberRoomIdDTO roomIdDTO) =>
            await this.GetMemberRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<GetMemberRoomDTO> GetMemberRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var responseRoom = Mapper.Map<GetMemberRoomDTO>(room);

            return responseRoom;
        }
        public async Task<GetMemberRoomDTO> GetMemberRoomByDomain(MemberRoomDomainDTO roomDomainDTO) =>
            await this.GetMemberRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<GetUserRoomsItemDTO[]> GetUserRoomsByOwnerId(ObjectId ownerId)
        {
            var rooms = await _roomRepository.GetRoomsByOwnerId(ownerId);
            var responseRooms = Mapper.Map<GetUserRoomsItemDTO[]>(rooms);

            return responseRooms;
        }
        public async Task<GetUserRoomsItemDTO[]> GetUserRoomsByOwnerId(UserIdDTO userIdDTO) =>
            await GetUserRoomsByOwnerId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<string> InsertRoom(PostUserRoomDTO postRoomDto)
        {
            var newRoom = Mapper.Map<Room>(postRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            return (await _roomRepository.InsertRoom(newRoom)).ToString();
        }

        public async Task<OperationErrorMessages> EditRoom(ObjectId roomId, PostUserRoomDTO postRoomDTO)
        {
            var outdatedRoom = await _roomRepository.GetRoomById(roomId);

            if (outdatedRoom == null)
                return OperationErrorMessages.RoomNotFound;

            if (outdatedRoom.Domain != postRoomDTO.Domain)
            {
                var alreadyTaken = await _roomRepository.GetRoomByDomain(postRoomDTO.Domain);

                if (alreadyTaken != null)
                    return OperationErrorMessages.DomainAlreadyTaken;
            }


            var updatedRoom = Mapper.Map<Room>(postRoomDTO);

            updatedRoom.Id = outdatedRoom.Id;
            updatedRoom.OwnerId = outdatedRoom.OwnerId;

            await _roomRepository.ReplaceRoom(roomId, updatedRoom);

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> EditRoom(UserRoomIdDTO roomIdDTO, PostUserRoomDTO roomUpdates) =>
            await EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

        public async Task RemoveMemberFromRoomByTokenById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomByIdById(roomId, memberId);

            if (!removed)
                throw new ResultException(OperationErrorMessages.TestNotFound);
        }
        public async Task RemoveMemberFromRoomByIdById(ObjectId roomId, MemberIdDTO memberIdDTO) =>
            await RemoveMemberFromRoomByTokenById(roomId, ObjectId.Parse(memberIdDTO.MemberId));
        public async Task RemoveMemberFromRoomByIdById(UserRoomIdDTO roomIdDTO, ObjectId memberId) =>
            await RemoveMemberFromRoomByTokenById(ObjectId.Parse(roomIdDTO.RoomId), memberId);
        public async Task RemoveMemberFromRoomByIdById(UserRoomIdDTO roomIdDTO, MemberIdDTO memberIdDTO) =>
            await RemoveMemberFromRoomByTokenById(ObjectId.Parse(roomIdDTO.RoomId), ObjectId.Parse(memberIdDTO.MemberId));

        public async Task DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);

            if (!deleted)
                throw new ResultException(OperationErrorMessages.RoomNotFound);
        }
        public async Task DeleteRoomById(UserRoomIdDTO roomIdDTO) =>
            await DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId)
        {
            var removed = await _roomRepository.RemoveMemberFromRoomByIdById(roomId, memberId);

            if(!removed)
                throw new ResultException(OperationErrorMessages.MemberNotFound);
        }
    }
}