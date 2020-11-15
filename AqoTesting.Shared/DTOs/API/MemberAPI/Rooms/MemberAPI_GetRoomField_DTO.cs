using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Rooms
{
    public class MemberAPI_GetRoomField_DTO
    {
        public string? Name { get; set; }
        public FieldType Type { get; set; }
        public bool IsRequired { get; set; }
        public string? Placeholder { get; set; }
        public string? Mask { get; set; }
        public string[]? Options { get; set; }
    }
}
