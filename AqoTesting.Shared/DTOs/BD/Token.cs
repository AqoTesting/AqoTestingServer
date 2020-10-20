using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.BD
{
    class Token
    {
        public string Value { get; set; }
        public DateTime CreationDate { get; set; }
        public byte Type { get; set; }
    }
}
