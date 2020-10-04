using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs
{
    public class ResponseObject
    {
        public bool Success { get; set; } = true;
        public dynamic Data { get; set; }
    }
}
