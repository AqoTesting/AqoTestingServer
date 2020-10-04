using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DTOs.BDModels
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId {get; set;}
        public DateTime CreationDate { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
    }
}
