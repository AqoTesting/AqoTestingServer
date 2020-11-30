using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Members
{
    public class UserAPI_GetMembersItemDTO
    {
        public string? Id { get; set; }
        public string? RoomId { get; set; }
        public string? UserId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsApproved { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }
}
