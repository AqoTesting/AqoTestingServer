using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Core.DTOs.BD
{
    public struct Section
    {
        public int Id { get; set; }
        //public int TestId { get; set; }
        public Question[] Questions { get; set; }
    }
}