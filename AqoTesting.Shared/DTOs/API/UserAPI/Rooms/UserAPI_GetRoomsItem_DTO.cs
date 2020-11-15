namespace AqoTesting.Shared.DTOs.API.UserAPI.Rooms
{
    public class UserAPI_GetRoomsItem_DTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Domain { get; set; }
        public string? OwnerId { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}
