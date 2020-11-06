using System.ComponentModel.DataAnnotations;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class RoomFieldInputDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public FieldType Type { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string Placeholder { get; set; }

        [StringLength(256)]
        public string Mask { get; set; }
    }
}
