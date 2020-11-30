using AqoTesting.Shared.Enums;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(ObjectId id, Role role);
        string GenerateToken(ObjectId id, Role role, ObjectId roomId);
    }
}
