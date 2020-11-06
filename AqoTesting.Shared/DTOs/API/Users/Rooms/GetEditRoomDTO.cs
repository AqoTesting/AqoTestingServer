using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces.DTO;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class GetEditRoomDTO
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public IUserRoomField[] Fields { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}