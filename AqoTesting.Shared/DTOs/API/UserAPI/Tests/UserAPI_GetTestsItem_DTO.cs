using System;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_GetTestsItem_DTO
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? OwnerId { get; set; }
        public string? RoomId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
    }
}
