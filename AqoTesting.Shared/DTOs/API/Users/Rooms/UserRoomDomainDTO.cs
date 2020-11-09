using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class UserRoomDomainDTO
    {
        [Required]
        [StringLength(63, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string RoomDomain { get; set; }
    }
}
