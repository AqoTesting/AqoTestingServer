﻿namespace AqoTesting.Shared.DTOs.DB.Rooms
{
    public struct RequestedField
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string? Placeholder { get; set; }
        public bool IsRequired { get; set; }
        public bool IsShowTable { get; set; }
    }
}