using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Rooms
{
    public class UserAPI_PostRoomDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string? Name { get; set; }

        [Required]
        [StringLength(63, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string? Domain { get; set; }

        [StringLength(4096)]
        public string? Description { get; set; }

        [Required]
        public UserAPI_RoomFieldDTO[]? Fields { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public bool? IsApproveManually { get; set; }

        [Required]
        public bool? IsRegistrationEnabled { get; set; }

        [MaxLength(128)]
        public UserAPI_RoomTagDTO[]? Tags { get; set; } = new UserAPI_RoomTagDTO[0];
    }
}
