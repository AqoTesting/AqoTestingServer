﻿using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        public TokenRepository(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task Add(Role role, ObjectId id, JwtSecurityToken jwt, int second)
        {
            await _cacheRepository.Set($"{role}:{id}:{jwt.EncodedPayload}", "1", second);
        }
        public async Task<bool> Check(Role role, ObjectId id, SecurityToken token)
        {
            JwtSecurityToken jwt = SecurityTokenConvertToJwtSecurityToken(token);
            long exist = await _cacheRepository.Exist($"{role}:{id}:{jwt.EncodedPayload}");
            if (exist == 0) return false;
            return true;
        }
        public async Task DelAll(Role role, ObjectId id)
        {
            var keys = await _cacheRepository.Keys($"{role}:{id}:*");
            if(keys.Length != 0) await _cacheRepository.DelAll(keys);
        }
        public async Task Del(Role role, ObjectId id, SecurityToken token)
        {
            JwtSecurityToken jwt = SecurityTokenConvertToJwtSecurityToken(token);
            await _cacheRepository.Del($"{role}:{id}:{jwt.EncodedPayload}");
        }
        private JwtSecurityToken SecurityTokenConvertToJwtSecurityToken(SecurityToken token)
        {
            return handler.ReadToken(handler.WriteToken(token)) as JwtSecurityToken;
        }
    }
}
