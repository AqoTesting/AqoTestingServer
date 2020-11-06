using System.ComponentModel.DataAnnotations;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class EditRoomDTO
    {
        [StringLength(64, MinimumLength = 1)]
        public string? Name { get; set; } = null;
        [StringLength(63, MinimumLength = 1)]
        public string? Domain { get; set; } = null;
        [StringLength(4096)]
        public string? Description { get; set; } = null;
        public RoomFieldDTO[]? Fields { get; set; } = null;
        public bool? IsActive { get; set; } = null;
        public bool? IsApproveManually { get; set; } = null;
        public bool? IsRegistrationEnabled { get; set; } = null;
    }
}