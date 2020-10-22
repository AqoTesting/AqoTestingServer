using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.DB.Users
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
