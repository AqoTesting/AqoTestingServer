using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.Interfaces.DTO
{
    public interface IFillInRoomField
    {
        string Name { get; set; }

        FieldType Type { get; set; }

        string Value { get; set; }
    }
}
