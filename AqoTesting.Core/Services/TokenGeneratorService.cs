using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Infrastructure;
using AqoTesting.Shared.Interfaces;
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
        public TokenGeneratorService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
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
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            _tokenRepository.Add(role, id, jwt, AuthOptions.LIFETIME);

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
