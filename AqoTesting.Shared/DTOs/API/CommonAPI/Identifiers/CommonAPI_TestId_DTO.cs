using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_TestIdDTO
    {
        [Required]
        [ObjectId]
        public string? TestId { get; set; }
    }
}
