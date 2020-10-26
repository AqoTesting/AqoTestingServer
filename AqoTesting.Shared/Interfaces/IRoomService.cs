using AqoTesting.Shared.DTOs.API.Members;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface IRoomService
    {
        Task<GetRoomDTO> GetRoomById(RoomIdDTO roomIdDTO);

        Task<Room> GetRoomByDomain(string domain);

        Task<GetRoomsItemDTO[]> GetRoomsByOwnerId(string ownerId);

        Task<string> InsertRoom(CreateRoomDTO newRoomDto);

        Task<GetRoomDTO> EditRoom(RoomIdDTO roomIdDTO, EditRoomDTO roomUpdates);

        Task RemoveMemberFromRoomByTokenById(RoomIdDTO roomIdDTO, MemberTokenDTO memberIdDTO);

        Task DeleteRoomById(RoomIdDTO roomIdDTO);
    }
}
