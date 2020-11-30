namespace AqoTesting.Shared.DTOs.API.UserAPI.Rooms
{
    public class UserAPI_GetRoomDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Domain { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public UserAPI_RoomFieldDTO[]? Fields { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}