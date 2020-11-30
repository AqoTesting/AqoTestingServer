using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI
{
    public class CommonAPI_BooleanValue_DTO
    {
        [Required]
        public bool? BooleanValue { get; set; }
    }
}
