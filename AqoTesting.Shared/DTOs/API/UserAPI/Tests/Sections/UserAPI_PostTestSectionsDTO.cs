using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostTestSectionsDTO
    {
        [Required]
        public Dictionary<string, UserAPI_PostTestSectionDTO>? Sections { get; set; } = new Dictionary<string, UserAPI_PostTestSectionDTO>();
    }
}
