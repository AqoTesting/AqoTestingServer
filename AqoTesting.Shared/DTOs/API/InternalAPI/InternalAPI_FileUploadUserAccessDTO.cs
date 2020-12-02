using AqoTesting.Shared.Attributes;
using AqoTesting.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.InternalAPI
{
    public class InternalAPI_FileUploadUserAccessDTO : IInternalAPI_PostDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 32)]
        [RegularExpression(@"^[A-z0-9]+$")]
        public string? AccessKey { get; set; }

        [Required]
        [ObjectId]
        public string? UserId { get; set; }
        [Required]
        [ObjectId]
        public string? RoomId { get; set; }
        [Required]
        [ObjectId]
        public string? TestId { get; set; }
    }
}
