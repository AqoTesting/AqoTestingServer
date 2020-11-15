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
        Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomById(ObjectId roomId);
        Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomById(RoomId_DTO roomIdDTO);

        Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomByDomain(string roomDomain);
        Task<UserAPI_GetRoom_DTO> UserAPI_GetRoomByDomain(UserAPI_RoomDomain_DTO roomDomainDTO);

        Task<UserAPI_GetRoomsItem_DTO[]> UserAPI_GetRoomsByOwnerId(ObjectId ownerId);
        Task<UserAPI_GetRoomsItem_DTO[]> UserAPI_GetRoomsByOwnerId(UserId_DTO userIdDTO);

        Task<string> UserAPI_InsertRoom(UserAPI_PostRoom_DTO postRoomDto);

        Task<OperationErrorMessages> UserAPI_EditRoom(ObjectId roomId, UserAPI_PostRoom_DTO postRoomDTO);
        Task<OperationErrorMessages> UserAPI_EditRoom(RoomId_DTO roomIdDTO, UserAPI_PostRoom_DTO roomUpdates);

        Task UserAPI_DeleteRoomById(ObjectId roomId);
        Task UserAPI_DeleteRoomById(RoomId_DTO roomIdDTO);

        Task UserAPI_RemoveMemberFromRoomById(ObjectId roomId, ObjectId memberId);


        Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(ObjectId roomId);
        Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(RoomId_DTO roomIdDTO);
        Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomById(string roomId);

        Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomByDomain(string roomDomain);
        Task<MemberAPI_GetRoom_DTO> MemberAPI_GetRoomByDomain(MemberAPI_RoomDomain_DTO roomDomainDTO);
    }
}
