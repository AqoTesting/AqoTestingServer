using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AqoTesting.Shared.Models
{
    public class AuthUser
    {
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
