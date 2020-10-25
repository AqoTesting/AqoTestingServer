using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class EditRoomDTO
    {
        [StringLength(64, MinimumLength = 1)]
        public string? Name { get; set; } = null;

        [StringLength(63, MinimumLength = 1)]
        public string? Domain { get; set; } = null;

        public RequestedFieldDTO[]? RequestedFields { get; set; } = null;

        public bool? IsDataRequired { get; set; } = null;

        public bool? IsActive { get; set; } = null;
    }
}