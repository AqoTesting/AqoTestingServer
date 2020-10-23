using System;

namespace AqoTesting.Shared.DTOs.DB
{
    class Token
    {
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }
        public byte Type { get; set; }
    }
}
