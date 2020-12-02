using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_AttemptIdDTO
    {
        [Required]
        [ObjectId]
        public string? AttemptId { get; set; }
    }
}
