using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace AqoTesting.Core.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private Dictionary<(Role, ObjectId), Dictionary<string, DateTime>> _storage;

        public TokenRepository()
        {
            _storage = new Dictionary<(Role, ObjectId), Dictionary<string, DateTime>>();
        }

        private Dictionary<string, DateTime> GetAccountTokenStorage(Role role, ObjectId accountId, bool create = false)
        {
            bool exists = _storage.TryGetValue((role, accountId), out Dictionary<string, DateTime> accountTokenStorage);
            if (exists)
                return accountTokenStorage;

            if(!create)
                return null;

            accountTokenStorage = new Dictionary<string, DateTime>();
            _storage.Add((role, accountId), accountTokenStorage);

            return accountTokenStorage;
        }

        public void Add(Role role, ObjectId accountId, JwtSecurityToken token, int expiresIn)
        {
            Dictionary<string, DateTime> tokenStorage = GetAccountTokenStorage(role, accountId, create: true);

            DateTime expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);

            tokenStorage.Add(token.EncodedPayload, expiresAt);
        }

        public bool Check(Role role, ObjectId accountId, JwtSecurityToken token)
        {
            var accountTokenStorage = GetAccountTokenStorage(role, accountId, create: false);
            if(accountTokenStorage == null)
                return false;

            if( !accountTokenStorage.TryGetValue(token.EncodedPayload, out DateTime expiresAt) )
                return false;

            if(DateTime.UtcNow > expiresAt)
            {
                Remove(role, accountId, token);
                return false;
            }
            
            return true;
        }

        public void RemoveAll(Role role, ObjectId id)
        {
            _storage.Remove((role, id));
        }

        public void Remove(Role role, ObjectId accountId, JwtSecurityToken token)
        {
            var userTokenStorage = GetAccountTokenStorage(role, accountId, create: false);
            if (userTokenStorage == null)
                return;

            userTokenStorage.Remove(token.EncodedPayload);

            if (userTokenStorage.Count == 0)
                _storage.Remove((role, accountId));
        }
    }
}
