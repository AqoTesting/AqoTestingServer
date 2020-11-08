using System.ComponentModel.DataAnnotations;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class PostRoomDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(63, MinimumLength = 1)]
        public string Domain { get; set; }

        [StringLength(4096)]
        public string Description { get; set; }

        [Required]
        public RoomFieldDTO[] Fields { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public bool? IsApproveManually { get; set; }

        [Required]
        public bool? IsRegistrationEnabled { get; set; }
    }
}
