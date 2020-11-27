using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostTestSections_DTO
    {
        [Required]
        public Dictionary<string, UserAPI_PostTestSection_DTO>? Sections { get; set; } = new Dictionary<string, UserAPI_PostTestSection_DTO>();
    }
}
