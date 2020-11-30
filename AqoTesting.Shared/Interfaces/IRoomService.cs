using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        #region UserAPI
        Task<(OperationErrorMessages, object)> CheckRoomDomainExists(string roomDomain);
        Task<(OperationErrorMessages, object)> CheckRoomDomainExists(CommonAPI_RoomDomainDTO roomDomainDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(CommonAPI_RoomIdDTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(CommonAPI_RoomDomainDTO roomDomainDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(ObjectId userId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(CommonAPI_UserIdDTO userIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_CreateRoom(UserAPI_PostRoomDTO postRoomDto);

        Task<(OperationErrorMessages, object)> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoomDTO postRoomDTO);
        Task<(OperationErrorMessages, object)> UserAPI_EditRoom(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostRoomDTO postRoomDTO);

        Task<(OperationErrorMessages, object)> UserAPI_SetRoomTags(ObjectId roomId, UserAPI_RoomTagDTO[] postRoomTagDTOs);
        Task<(OperationErrorMessages, object)> UserAPI_SetRoomTags(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostRoomTagsDTO postRoomTagsDTO);

        Task<(OperationErrorMessages, object)> UserAPI_DeleteRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_DeleteRoomById(CommonAPI_RoomIdDTO roomIdDTO);
        #endregion

        #region MemberAPI
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(CommonAPI_RoomIdDTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(CommonAPI_RoomDomainDTO roomDomainDTO);
        #endregion
    }
}
