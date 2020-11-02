using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.Interfaces.DTO
{
    public interface IGetMemberRoomField
    {
        string Name { get; set; }

        FieldType Type { get; set; }

        bool IsRequired { get; set; }

        bool IsShowTable { get; set; }
    }
}
