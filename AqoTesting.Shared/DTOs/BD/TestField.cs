using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.BD
{
    public struct TestField
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Name { get; set; }
        public string? Placeholder { get; set; }
    }
}
