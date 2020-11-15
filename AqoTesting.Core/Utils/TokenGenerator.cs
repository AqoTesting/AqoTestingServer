using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AqoTesting.Core.Utils
{
    public static class TokenGenerator
    {
        public static string GenerateToken(ObjectId id, Role role)
        {
            var identity = GetIdentity(id, role, ObjectId.Empty, false);

            return GetTokenByIdentity(identity);
        }
        public static string GenerateToken(ObjectId id, Role role, ObjectId roomId, bool isChecked = true)
        {
            var identity = GetIdentity(id, role, roomId, isChecked);

            return GetTokenByIdentity(identity);
        }

        private static string GetTokenByIdentity(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private static ClaimsIdentity GetIdentity(ObjectId id, Role role, ObjectId roomId, bool isChecked)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            if(role == Role.Member)
            {
                claims.Add(new Claim("isChecked", isChecked.ToString()));
                claims.Add(new Claim("roomId", roomId.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
