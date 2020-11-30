using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Members
{
    public class UserAPI_PostMemberTagsDTO
    {
        [Required]
        [MaxLength(16)]
        public UserAPI_MemberTagDTO[]? Tags { get; set; }
    }
}
