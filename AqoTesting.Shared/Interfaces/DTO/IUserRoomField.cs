using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.Interfaces.DTO
{
    public interface IUserRoomField
    {
        string Name { get; set; }

        FieldType Type { get; set; }

        bool IsRequired { get; set; }
    }
}
