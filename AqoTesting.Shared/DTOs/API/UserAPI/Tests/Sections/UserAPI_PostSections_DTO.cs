using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_PostSections_DTO
    {
        [Required]
        [MinLength(1)]
        public Dictionary<int, UserAPI_PostSection_DTO>? Sections { get; }
    }
}
