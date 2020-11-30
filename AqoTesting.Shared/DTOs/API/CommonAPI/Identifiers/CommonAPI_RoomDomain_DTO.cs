using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_RoomDomain_DTO
    {
        [Required]
        [StringLength(63, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string? RoomDomain { get; set; }
    }
}
