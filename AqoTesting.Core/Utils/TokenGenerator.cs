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
        public static TokenDTO GenerateToken(ObjectId id, Role role = Role.User, bool isChecked = true)
        {
            var identity = GetIdentity(id, role, isChecked);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenDTO { Token = encodedJwt };
        }

        private static ClaimsIdentity GetIdentity(ObjectId id, Role role, bool isChecked)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            if(role == Role.Member)
            {
                claims.Add(new Claim("isChecked", isChecked.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
