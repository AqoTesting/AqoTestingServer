using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_TestSectionId_DTO
    {
        [Required]
        public string SectionId { get; set; }
    }
}
