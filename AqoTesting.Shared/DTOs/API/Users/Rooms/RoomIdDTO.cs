using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class RoomIdDTO
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [RegularExpression(@"^[0-9abcdef]+$")]
        public string Id { get; set; }
    }
}
