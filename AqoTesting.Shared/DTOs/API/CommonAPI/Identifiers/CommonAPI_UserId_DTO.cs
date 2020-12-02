using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_UserIdDTO
    {
        [Required]
        [ObjectId]
        public string? UserId { get; set; }
    }
}
