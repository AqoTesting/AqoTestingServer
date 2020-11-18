using AqoTesting.Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITokenRepository
    {
        void Add(Role role, ObjectId id, JwtSecurityToken jwt, int second);
        Task<bool> Check(Role role, ObjectId id, SecurityToken token);
        void DelAll(Role role, ObjectId id);
        void Del(Role role, ObjectId id, SecurityToken token);
    }
}
