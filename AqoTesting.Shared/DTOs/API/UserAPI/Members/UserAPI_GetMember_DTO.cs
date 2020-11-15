using System.Collections.Generic;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Members
{
    public class UserAPI_GetMember_DTO
    {
        public string? Id { get; set; }
        public string? RoomId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsChecked { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }
}
