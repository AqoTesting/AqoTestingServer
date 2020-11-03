using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
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

        Task<string> InsertRoom(CreateRoomDTO newRoomDto);

        Task<GetRoomDTO> EditRoom(ObjectId roomId, EditRoomDTO roomUpdates);
        Task<GetRoomDTO> EditRoom(RoomIdDTO roomIdDTO, EditRoomDTO roomUpdates);

        Task RemoveMemberFromRoomByTokenById(ObjectId roomId, string memberToken);
        Task RemoveMemberFromRoomByTokenById(ObjectId roomId, MemberTokenDTO memberTokenDTO);
        Task RemoveMemberFromRoomByTokenById(RoomIdDTO roomIdDTO, string memberToken);
        Task RemoveMemberFromRoomByTokenById(RoomIdDTO roomIdDTO, MemberTokenDTO memberTokenDTO);

        Task DeleteRoomById(ObjectId roomId);
        Task DeleteRoomById(RoomIdDTO roomIdDTO);
    }
}
