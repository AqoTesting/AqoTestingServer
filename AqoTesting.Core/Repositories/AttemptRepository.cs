using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        Dictionary<ObjectId, AttemptsDB_AttemptDTO> _internalByIdCache;
        Dictionary<ObjectId, ObjectId?> _internalActiveByMemberIdCache;
        Dictionary<ObjectId, ObjectId[]> _internalByTestIdCache;
        Dictionary<ObjectId, ObjectId[]> _internalByMemberIdCache;
        Dictionary<(ObjectId, ObjectId), ObjectId[]> _internalByTestIdAndMemberIdCache;

        IWorkContext _workContext;
        ICacheRepository _redisCache;
        public AttemptRepository(IWorkContext workContext, ICacheRepository cache)
        {
            _workContext = workContext;
            _redisCache = cache;

            _internalByIdCache = new Dictionary<ObjectId, AttemptsDB_AttemptDTO>();
            _internalActiveByMemberIdCache = new Dictionary<ObjectId, ObjectId?>();
            _internalByTestIdCache = new Dictionary<ObjectId, ObjectId[]>();
            _internalByMemberIdCache = new Dictionary<ObjectId, ObjectId[]>();
            _internalByTestIdAndMemberIdCache = new Dictionary<(ObjectId, ObjectId), ObjectId[]>();
        }

        public async Task<AttemptsDB_AttemptDTO> GetAttemptById(ObjectId attemptId)
        {
            AttemptsDB_AttemptDTO attempt;

            if(_internalByIdCache.ContainsKey(attemptId))
                attempt = _internalByIdCache[attemptId];
            else
            {
                attempt = await AttemptWorker.GetAttemptById(attemptId);

                _internalByIdCache.Add(attemptId, attempt);
            }

            return attempt;
        }

        public async Task<AttemptsDB_AttemptDTO> GetActiveAttemptByMemberId(ObjectId memberId)
        {
            AttemptsDB_AttemptDTO attempt;

            if(!_internalActiveByMemberIdCache.ContainsKey(memberId) || _internalActiveByMemberIdCache[memberId] != null && !_internalByIdCache.ContainsKey(_internalActiveByMemberIdCache[memberId].Value))
            {
                attempt = await _redisCache.Get($"MemberActiveAttempt:{memberId}",
                    async () => await AttemptWorker.GetActiveAttemptByMemberId(memberId));

                if(attempt != null)
                {
                    _internalActiveByMemberIdCache.TryAdd(memberId, attempt.Id);
                    _internalByIdCache.TryAdd(attempt.Id, attempt);
                }
                else
                    _internalActiveByMemberIdCache.TryAdd(memberId, null);
            }
            else
                attempt = _internalActiveByMemberIdCache[memberId] != null ?
                    _internalByIdCache[_internalActiveByMemberIdCache[memberId].Value] :
                null;

            return attempt;
        }
            

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestId(ObjectId testId)
        {
            AttemptsDB_AttemptDTO[] attempts = new AttemptsDB_AttemptDTO[0];

            var cached = _internalByTestIdCache.ContainsKey(testId);

            if(cached)
                attempts = _internalByTestIdCache[testId]
                    .Where(attemptId => _internalByIdCache.ContainsKey(attemptId))
                        .Select(attemptId =>
                            _internalByIdCache[attemptId])
                                .ToArray();

            if(!cached || attempts.Length != _internalByTestIdCache[testId].Length)
            {
                attempts = await AttemptWorker.GetAttemptsByTestId(testId);

                var attemptIds = new ObjectId[attempts.Length];
                for(var i = 0; i < attempts.Length; i++)
                {
                    _internalByIdCache.TryAdd(attempts[i].Id, attempts[i]);
                    attemptIds[i] = attempts[i].Id;
                }
                _internalByTestIdCache.TryAdd(testId, attemptIds);
            }

            return attempts;
        }

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByMemberId(ObjectId memberId)
        {
            AttemptsDB_AttemptDTO[] attempts = new AttemptsDB_AttemptDTO[0];

            var cached = _internalByMemberIdCache.ContainsKey(memberId);

            if(cached)
                attempts = _internalByMemberIdCache[memberId]
                    .Where(attemptId => _internalByIdCache.ContainsKey(attemptId))
                        .Select(attemptId =>
                            _internalByIdCache[attemptId])
                                .ToArray();

            if(!cached || attempts.Length != _internalByMemberIdCache[memberId].Length)
            {
                attempts = await AttemptWorker.GetAttemptsByMemberId(memberId);

                var attemptIds = new ObjectId[attempts.Length];
                for(var i = 0; i < attempts.Length; i++)
                {
                    _internalByIdCache.TryAdd(attempts[i].Id, attempts[i]);
                    attemptIds[i] = attempts[i].Id;
                }
                _internalByMemberIdCache.TryAdd(memberId, attemptIds);
            }

            return attempts;
        }
        

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestIdAndMemberId (ObjectId testId, ObjectId memberId)
        {
            AttemptsDB_AttemptDTO[] attempts = new AttemptsDB_AttemptDTO[0];

            var cached = _internalByTestIdAndMemberIdCache.ContainsKey((testId, memberId));

            if(cached)
                attempts = _internalByTestIdAndMemberIdCache[(testId, memberId)]
                    .Where(attemptId => _internalByIdCache.ContainsKey(attemptId))
                        .Select(attemptId =>
                            _internalByIdCache[attemptId])
                                .ToArray();

            if(!cached || attempts.Length != _internalByTestIdAndMemberIdCache[(testId, memberId)].Length)
            {
                attempts = await AttemptWorker.GetAttemptsByTestIdAndMemberId(testId, memberId);

                var attemptIds = new ObjectId[attempts.Length];
                for(var i = 0; i < attempts.Length; i++)
                {
                    _internalByIdCache.TryAdd(attempts[i].Id, attempts[i]);
                    attemptIds[i] = attempts[i].Id;
                }
                _internalByTestIdAndMemberIdCache.TryAdd((testId, memberId), attemptIds);
            }

            return attempts;
        }

        public async Task<ObjectId> InsertAttempt(AttemptsDB_AttemptDTO newAttempt)
        {
            _internalActiveByMemberIdCache = new Dictionary<ObjectId, ObjectId?>();
            _internalByTestIdCache = new Dictionary<ObjectId, ObjectId[]>();
            _internalByMemberIdCache = new Dictionary<ObjectId, ObjectId[]>();
            _internalByTestIdAndMemberIdCache = new Dictionary<(ObjectId, ObjectId), ObjectId[]>();

            return await AttemptWorker.InsertAttempt(newAttempt);
        }

        public async Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue, ObjectId? memberId = null)
        {
            if (memberId == null)
                if(_workContext.MemberId != null)
                    memberId = _workContext.MemberId.Value;
                else
                {
                    var attempt = await AttemptWorker.GetAttemptById(attemptId);
                    if(attempt == null)
                        return false;

                    memberId = attempt.MemberId;
                }

            await _redisCache.Del($"MemberActiveAttempt:{memberId.Value}");
            _internalByIdCache.Remove(attemptId);

            return await AttemptWorker.SetProperty(attemptId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties, ObjectId? memberId = null)
        {
            if (memberId == null)
                memberId = _workContext.MemberId.Value;

            await _redisCache.Del($"MemberActiveAttempt:{memberId}");
            _internalByIdCache.Remove(attemptId);

            return await AttemptWorker.SetProperties(attemptId, properties);
        }

        public async Task<bool> Delete(ObjectId attemptId)
        {
            var attempt = _internalByIdCache.ContainsKey(attemptId) ?
                _internalByIdCache[attemptId] :
            await AttemptWorker.GetAttemptById(attemptId);

            if(attempt == null)
                return false;

            var memberId = attempt.MemberId;

            await _redisCache.Del($"MemberActiveAttempt:{memberId}");

            _internalActiveByMemberIdCache.Remove(memberId);
            _internalByIdCache.Remove(attemptId);

            return await AttemptWorker.DeleteAttempt(attemptId);
        }

        public async Task DeleteAttemptsByMemberId(ObjectId memberId)
        {
            await _redisCache.Del($"MemberActiveAttempt:{memberId}");

            if(_internalByMemberIdCache.ContainsKey(memberId))
                _internalByMemberIdCache[memberId].Select(attemptId =>
                    _internalByIdCache.Remove(attemptId));

            _internalByMemberIdCache.Remove(memberId);

            await AttemptWorker.DeleteAttemptsByMemberId(memberId);
        }
    }
}