using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(CommonAPI_RoomDomain_DTO roomDomainDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(ObjectId userId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByUserId(CommonAPI_UserId_DTO userIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_CreateRoom(UserAPI_PostRoom_DTO postRoomDto);

        Task<OperationErrorMessages> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoom_DTO postRoomDTO);
        Task<OperationErrorMessages> UserAPI_EditRoom(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates);
        Task<OperationErrorMessages> UserAPI_DeleteRoomById(ObjectId roomId);
        Task<OperationErrorMessages> UserAPI_DeleteRoomById(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(CommonAPI_RoomDomain_DTO roomDomainDTO);
    }
}
