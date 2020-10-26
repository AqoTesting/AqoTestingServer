using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Members
{
    public class MemberTokenDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string MemberToken { get; set; }
    }
}
