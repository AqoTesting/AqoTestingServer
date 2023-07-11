using AqoTesting.Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITokenRepository
    {
        void Add(Role role, ObjectId accountId, JwtSecurityToken token, int expiresIn);
        bool Check(Role role, ObjectId accountId, JwtSecurityToken token);
        void RemoveAll(Role role, ObjectId accountId);
        void Remove(Role role, ObjectId accountId, JwtSecurityToken token);
    }
}
