using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Domain.Utils
{
    public class BsonID
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}