using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Rooms
{
    public class UserAPI_PostRoomTagsDTO
    {
        [Required]
        [MaxLength(128)]
        public UserAPI_RoomTagDTO[]? Tags { get; set; }
    }
}
