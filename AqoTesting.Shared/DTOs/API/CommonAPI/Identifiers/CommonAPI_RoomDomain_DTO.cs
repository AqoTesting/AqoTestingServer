using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_RoomDomainDTO
    {
        [Required]
        [StringLength(63, MinimumLength = 1)]
        [RegularExpression(@"^[A-z0-9_]+$")]
        public string? RoomDomain { get; set; }
    }
}
