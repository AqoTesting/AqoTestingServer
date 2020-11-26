namespace AqoTesting.Shared.DTOs.API.MemberAPI.Rooms
{
    public class MemberAPI_GetRoom_DTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Domain { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public MemberAPI_GetRoomField_DTO[]? Fields { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}