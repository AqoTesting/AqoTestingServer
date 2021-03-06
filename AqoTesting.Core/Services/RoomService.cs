﻿using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.CommonAPI;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using AqoTesting.Shared.DTOs.DB.Rooms;

namespace AqoTesting.Core.Services
{
    public class RoomService : ServiceBase, IRoomService
    {
        IRoomRepository _roomRepository;
        IUserRepository _userRepository;
        IMemberRepository _memberRepository;
        ITestRepository _testRepository;
        IAttemptRepository _attemptRepository;
        IWorkContext _workContext;

        public RoomService(IRoomRepository roomRespository, IUserRepository userRepository, IMemberRepository memberRepository, ITestRepository testRepository, IAttemptRepository attemptRepository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _testRepository = testRepository;
            _attemptRepository = attemptRepository;
            _workContext = workContext;
        }

        #region User API
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(string roomDomain)
        {
            var roomExists = await _roomRepository.GetRoomByDomain(roomDomain);

            var booleanResponseDTO = new CommonAPI_BooleanValueDTO { BooleanValue = roomExists != null };

            return (OperationErrorMessages.NoError, booleanResponseDTO);
        }
        public async Task<(OperationErrorMessages, object)> CheckRoomDomainExists(CommonAPI_RoomDomainDTO roomDomainDTO) =>
            await this.CheckRoomDomainExists(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            var getRoomDTO = Mapper.Map<UserAPI_GetRoomDTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(CommonAPI_RoomIdDTO roomIdDTO) =>
            await UserAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            var getRoomDTO = Mapper.Map<UserAPI_GetRoomDTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(CommonAPI_RoomDomainDTO roomDomainDTO) =>
            await this.UserAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);

        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(ObjectId userId)
        {
            var userExists = await _userRepository.GetUserById(userId);
            if(userExists == null)
                return (OperationErrorMessages.UserNotFound, null);

            var rooms = await _roomRepository.GetRoomsByUserId(userId);
            var getRoomsItemDTOs = Mapper.Map<UserAPI_GetRoomsItemDTO[]>(rooms);

            return(OperationErrorMessages.NoError,  getRoomsItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(CommonAPI_UserIdDTO userIdDTO) =>
            await UserAPI_GetRoomsByUserId(ObjectId.Parse(userIdDTO.UserId));

        public async Task<(OperationErrorMessages, object)> UserAPI_CreateRoom(UserAPI_PostRoomDTO postRoomDto)
        {
            var domainTaken = await _roomRepository.GetRoomByDomain(postRoomDto.Domain);
            if(domainTaken != null)
                return (OperationErrorMessages.DomainAlreadyTaken, null);

            var newRoom = Mapper.Map<RoomsDB_RoomDTO>(postRoomDto);
            newRoom.UserId = _workContext.UserId.Value;

            var newRoomId = await _roomRepository.InsertRoom(newRoom);
            var newRoomIdDTO = new CommonAPI_RoomIdDTO { RoomId = newRoomId.ToString() };

            return (OperationErrorMessages.NoError, newRoomIdDTO);
        }

        public async Task<(OperationErrorMessages, object)> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoomDTO postRoomDTO)
        {
            var outdatedRoom = await _roomRepository.GetRoomById(roomId);

            if(outdatedRoom.Domain != postRoomDTO.Domain)
            {
                var alreadyTaken = await _roomRepository.GetRoomByDomain(postRoomDTO.Domain);

                if(alreadyTaken != null)
                    return (OperationErrorMessages.DomainAlreadyTaken, null);
            }


            var updatedRoom = Mapper.Map<RoomsDB_RoomDTO>(postRoomDTO);

            updatedRoom.Id = outdatedRoom.Id;
            updatedRoom.UserId = outdatedRoom.UserId;

            await _roomRepository.ReplaceRoom(updatedRoom);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_EditRoom(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostRoomDTO postRoomDTO) =>
            await UserAPI_EditRoom(ObjectId.Parse(roomIdDTO.RoomId), postRoomDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_SetRoomTags(ObjectId roomId, UserAPI_RoomTagDTO[] postRoomTagDTOs)
        {
            var tags = Mapper.Map<RoomsDB_TagDTO[]>(postRoomTagDTOs);
            await _roomRepository.SetTags(roomId, tags);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_SetRoomTags(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostRoomTagsDTO postRoomTagsDTO) =>
            await this.UserAPI_SetRoomTags(ObjectId.Parse(roomIdDTO.RoomId), postRoomTagsDTO.Tags);

        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteRoomById(ObjectId roomId)
        {
            var deleted = await _roomRepository.DeleteRoomById(roomId);
            if(!deleted)
                return (OperationErrorMessages.RoomNotFound, null);

            await _testRepository.DeleteTestsByRoomId(roomId);
            await _attemptRepository.DeleteAttemptsByRoomId(roomId);
            await _memberRepository.DeleteMembersByRoomId(roomId);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_DeleteRoomById(CommonAPI_RoomIdDTO roomIdDTO) =>
            await UserAPI_DeleteRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoomDTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(CommonAPI_RoomIdDTO roomIdDTO) =>
            await this.MemberAPI_GetRoomById(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain)
        {
            var room = await _roomRepository.GetRoomByDomain(roomDomain);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var getRoomDTO = Mapper.Map<MemberAPI_GetRoomDTO>(room);

            return (OperationErrorMessages.NoError, getRoomDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(CommonAPI_RoomDomainDTO roomDomainDTO) =>
            await this.MemberAPI_GetRoomByDomain(roomDomainDTO.RoomDomain);
        #endregion
    }
}