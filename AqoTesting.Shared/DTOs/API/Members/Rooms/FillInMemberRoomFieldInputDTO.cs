using System;
using System.Collections.Generic;
using System.Text;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces.DTO;

namespace AqoTesting.Shared.DTOs.API.Members.Rooms
{
    public class FillInMemberRoomRequestedFieldInputDTO : IFillInRoomField
    {
        public string Name { get; set; }

        public FieldType Type { get; set; }

        public string Value { get; set; }
    }
}
