using System.ComponentModel.DataAnnotations;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public struct RoomFieldSelectDTO : IUserRoomField
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public FieldType Type { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool IsShowTable { get; set; }

        [Required]
        public bool IsKey { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(32)]
        public string[] Options { get; set; }
    }
}
