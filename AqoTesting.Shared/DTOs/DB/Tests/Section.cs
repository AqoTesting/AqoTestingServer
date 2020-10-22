using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.DB.Tests
{
    public class Section
    {
        public int Id { get; set; }
        //public int TestId { get; set; }
        public Question[] Questions { get; set; }
    }
}
