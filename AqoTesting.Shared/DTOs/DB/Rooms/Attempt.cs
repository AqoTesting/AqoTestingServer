using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.DB.Rooms
{
    public class Attempt
    {
        public ObjectId TestId { get; set; }
        public int SectionId { get; set; }
        public int QuestionId { get; set; }
        public object Answer { get; set; }
    }
}
