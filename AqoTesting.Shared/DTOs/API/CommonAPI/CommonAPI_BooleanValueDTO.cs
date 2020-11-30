using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI
{
    public class CommonAPI_BooleanValueDTO
    {
        [Required]
        public bool? BooleanValue { get; set; }
    }
}
