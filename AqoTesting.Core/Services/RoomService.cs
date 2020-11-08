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

            var responseRoom = Mapper.Map<GetRoomDTO>(room);

            // костыль
            if(responseRoom != null && responseRoom.Fields != null)
                for(var i = 0; i < responseRoom.Fields.Length; i++)
                    if (responseRoom.Fields[i].Options != null && responseRoom.Fields[i].Options.Length == 0)
                        responseRoom.Fields[i].Options = null;

            return responseRoom;
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

        public async Task<string> InsertRoom(PostRoomDTO postRoomDto)
        {
            var newRoom = Mapper.Map<Room>(postRoomDto);
            newRoom.OwnerId = _workContext.UserId;

            return (await _roomRepository.InsertRoom(newRoom)).ToString();
        }

        public async Task<OperationErrorMessages> EditRoom(ObjectId roomId, PostRoomDTO postRoomDTO)
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
        public async Task<OperationErrorMessages> EditRoom(RoomIdDTO roomIdDTO, PostRoomDTO roomUpdates) =>
            await EditRoom(ObjectId.Parse(roomIdDTO.RoomId), roomUpdates);

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