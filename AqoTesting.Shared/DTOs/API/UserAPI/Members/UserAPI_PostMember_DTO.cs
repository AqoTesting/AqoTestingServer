using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Members
{
    public class UserAPI_PostMember_DTO
    {
        [Required]
        public Dictionary<string, string>? Fields { get; set; }
    }
}
