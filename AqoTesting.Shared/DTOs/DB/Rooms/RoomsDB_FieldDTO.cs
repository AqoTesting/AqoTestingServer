﻿using AqoTesting.Core.Utils;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace AqoTesting.Shared.DTOs.DB.Rooms
{
    public class RoomsDB_FieldDTO
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(BsonDocumentConverter))]
        public BsonDocument Data { get; set; }
    }
}
