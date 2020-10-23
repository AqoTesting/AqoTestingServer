using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public struct RequestedFieldDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string Key { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string? Placeholder { get; set; }

        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public bool IsShowTable { get; set; }
    }
}
