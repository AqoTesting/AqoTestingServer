using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class GetUserMembersItemDTO
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string Login { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsChecked { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}
