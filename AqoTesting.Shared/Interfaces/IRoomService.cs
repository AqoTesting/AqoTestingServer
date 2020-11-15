using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomById(RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomByDomain(RoomDomain_DTO roomDomainDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByOwnerId(ObjectId ownerId);
        Task<(OperationErrorMessages, object)> UserAPI_GetRoomsByOwnerId(UserId_DTO userIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_InsertRoom(UserAPI_PostRoom_DTO postRoomDto);

        Task<OperationErrorMessages> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoom_DTO postRoomDTO);
        Task<OperationErrorMessages> UserAPI_EditRoom(RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates);
        Task<OperationErrorMessages> UserAPI_DeleteRoomById(ObjectId roomId);
        Task<OperationErrorMessages> UserAPI_DeleteRoomById(RoomId_DTO roomIdDTO);

        Task<OperationErrorMessages> UserAPI_RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId);


        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomById(RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(string roomDomain);
        Task<(OperationErrorMessages, object)> MemberAPI_GetRoomByDomain(RoomDomain_DTO roomDomainDTO);
    }
}
