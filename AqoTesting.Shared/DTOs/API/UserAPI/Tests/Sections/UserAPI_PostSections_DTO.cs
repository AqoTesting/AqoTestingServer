using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostSections_DTO
    {
        [Required]
        public Dictionary<string, UserAPI_PostSection_DTO>? Sections { get; set; } = new Dictionary<string, UserAPI_PostSection_DTO>();
    }
}
