using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public class SectionWithQuestions : Section
    {
        public Question[]? Questions { get; set; }
    }
}
