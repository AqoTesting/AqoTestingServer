using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(ObjectId id, Role role);
        string GenerateToken(ObjectId id, Role role, ObjectId roomId, bool isChecked = true);
    }
}
