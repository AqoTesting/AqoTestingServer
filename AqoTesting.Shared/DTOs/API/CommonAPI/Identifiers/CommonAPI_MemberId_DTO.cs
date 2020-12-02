using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_MemberIdDTO
    {
        [Required]
        [ObjectId]
        public string? MemberId { get; set; }
    }
}
