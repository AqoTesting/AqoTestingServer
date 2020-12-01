using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Infrastructure;
using AqoTesting.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AqoTesting.Core.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IOptions<AuthOptionsConfig> _authOptionsConfig;
        public TokenGeneratorService(ITokenRepository tokenRepository, IOptions<AuthOptionsConfig> authOptionsConfig)
        {
            _tokenRepository = tokenRepository;
            _authOptionsConfig = authOptionsConfig;
        }
        public string GenerateToken(ObjectId id, Role role)
        {
            var identity = GetIdentity(id, role, ObjectId.Empty);

            return GetTokenByIdentity(identity, role, id);
        }
        public string GenerateToken(ObjectId id, Role role, ObjectId roomId)
        {
            var identity = GetIdentity(id, role, roomId);

            return GetTokenByIdentity(identity, role, id);
        }

        private string GetTokenByIdentity(ClaimsIdentity identity, Role role, ObjectId id)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _authOptionsConfig.Value.Issuer,
                    audience: _authOptionsConfig.Value.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(_authOptionsConfig.Value.LifeTime)),
                    signingCredentials: new SigningCredentials(_authOptionsConfig.Value.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            _tokenRepository.Add(role, id, jwt, _authOptionsConfig.Value.LifeTime);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private ClaimsIdentity GetIdentity(ObjectId id, Role role, ObjectId roomId)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            if (role == Role.Member)
            {
                claims.Add(new Claim("roomId", roomId.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
