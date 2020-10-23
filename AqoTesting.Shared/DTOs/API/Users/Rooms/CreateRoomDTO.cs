using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class CreateRoomDTO
    {
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(63, MinimumLength = 1)]
        public string Domain { get; set; }

        [Required]
        public RequestedFieldDTO[] RequestedFields { get; set; }
    }
}
