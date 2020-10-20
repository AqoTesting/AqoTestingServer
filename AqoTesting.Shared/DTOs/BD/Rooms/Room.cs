using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.BD.Rooms
{
    public class Room
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public List<Member> Members { get; set; }
        public List<ObjectId> TestIds { get; set; }
        public ObjectId OwnerId { get; set; }
        public bool IsDataRequire { get; set; }
        public RequestedField[] RequestedFields { get; set; }
        public bool IsActive { get; set; }
    }
}
