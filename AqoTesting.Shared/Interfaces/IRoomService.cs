using AqoTesting.Shared.DTOs.API.Members;
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
        Task<GetRoomDTO> GetRoomById(ObjectId roomId);
        Task<GetRoomDTO> GetRoomById(RoomIdDTO roomIdDTO);

        Task<Room> GetRoomByDomain(string domain);

        Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(ObjectId ownerId);
        Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(UserIdDTO userIdDTO);

        Task<string> InsertRoom(PostRoomDTO newRoomDto);

        Task<OperationErrorMessages> EditRoom(ObjectId roomId, PostRoomDTO roomUpdates);
        Task<OperationErrorMessages> EditRoom(RoomIdDTO roomIdDTO, PostRoomDTO roomUpdates);

        Task RemoveMemberFromRoomByIdById(ObjectId roomId, ObjectId memberId);
        Task RemoveMemberFromRoomByIdById(ObjectId roomId, MemberIdDTO memberIdDTO);
        Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, ObjectId memberId);
        Task RemoveMemberFromRoomByIdById(RoomIdDTO roomIdDTO, MemberIdDTO memberIdDTO);

        Task DeleteRoomById(ObjectId roomId);
        Task DeleteRoomById(RoomIdDTO roomIdDTO);
    }
}
