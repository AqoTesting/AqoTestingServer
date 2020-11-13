using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        Task<GetUserRoomDTO> GetUserRoomById(ObjectId roomId);
        Task<GetUserRoomDTO> GetUserRoomById(RoomIdDTO roomIdDTO);

        Task<GetUserRoomDTO> GetUserRoomByDomain(string roomDomain);
        Task<GetUserRoomDTO> GetUserRoomByDomain(UserRoomDomainDTO roomDomainDTO);

        Task<GetMemberRoomDTO> GetMemberRoomById(ObjectId roomId);
        Task<GetMemberRoomDTO> GetMemberRoomById(MemberRoomIdDTO roomIdDTO);
        Task<GetMemberRoomDTO> GetMemberRoomById(string roomId);

        Task<GetMemberRoomDTO> GetMemberRoomByDomain(string roomDomain);
        Task<GetMemberRoomDTO> GetMemberRoomByDomain(MemberRoomDomainDTO roomDomainDTO);

        Task<GetUserRoomsItemDTO[]> GetUserRoomsByOwnerId(ObjectId ownerId);
        Task<GetUserRoomsItemDTO[]> GetUserRoomsByOwnerId(UserIdDTO userIdDTO);

        Task<GetUserMembersItemDTO[]> GetUserMembersByRoomId(ObjectId roomId);
        Task<GetUserMembersItemDTO[]> GetUserMembersByRoomId(RoomIdDTO roomIdDTO);

        Task<string> InsertRoom(PostUserRoomDTO newRoomDto);

        Task<OperationErrorMessages> EditRoom(ObjectId roomId, PostUserRoomDTO roomUpdates);
        Task<OperationErrorMessages> EditRoom(RoomIdDTO roomIdDTO, PostUserRoomDTO roomUpdates);

        Task RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId);
        Task RemoveMemberFromRoomByIdById(ObjectId roomId, MemberIdDTO memberIdDTO);
        Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, ObjectId memberId);
        Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, MemberIdDTO memberIdDTO);

        Task DeleteRoomById(ObjectId roomId);
        Task DeleteRoomById(RoomIdDTO roomIdDTO);
    }
}
