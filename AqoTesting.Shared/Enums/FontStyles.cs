using System;

namespace AqoTesting.Shared.Enums
{
    [Flags]
    public enum FontStyles : byte
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Crossed = 8
    }
}
