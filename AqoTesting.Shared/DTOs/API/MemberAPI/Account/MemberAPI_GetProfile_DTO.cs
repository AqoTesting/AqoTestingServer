using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Account
{
    public class MemberAPI_GetProfile_DTO
    {
        public string? Id { get; set; }
        public string? RoomId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public bool IsApproved { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }
}
