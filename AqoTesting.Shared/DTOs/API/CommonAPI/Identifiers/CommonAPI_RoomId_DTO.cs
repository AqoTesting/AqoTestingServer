using AqoTesting.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_RoomIdDTO
    {
        [Required]
        [ObjectId]
        public string? RoomId { get; set; }
    }
}
