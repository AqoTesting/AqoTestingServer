using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        IWorkContext _workContext;
        ICacheRepository _cache;
        public AttemptRepository(IWorkContext workContext, ICacheRepository cache)
        {
            _workContext = workContext;
            _cache = cache;
        }

        public async Task<AttemptsDB_Attempt_DTO> GetAttemptById(ObjectId attemptId) =>
            await AttemptWorker.GetAttemptById(attemptId);

        public async Task<AttemptsDB_Attempt_DTO> GetActiveAttemptByMemberId(ObjectId memberId) =>
            await _cache.Get($"MemberActiveAttempt:{memberId}",
                async () => await AttemptWorker.GetActiveAttemptByMemberId(memberId));

        public async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestId(ObjectId testId) =>
            await AttemptWorker.GetAttemptsByTestId(testId);

        public async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByMemberId(ObjectId memberId) =>
            await AttemptWorker.GetAttemptsByMemberId(memberId);

        public async Task<AttemptsDB_Attempt_DTO[]> GetAttemptsByTestIdAndMemberId (ObjectId testId, ObjectId memberId) =>
            await AttemptWorker.GetAttemptsByTestIdAndMemberId(testId, memberId);

        public async Task<ObjectId> InsertAttempt(AttemptsDB_Attempt_DTO newAttempt) =>
            await AttemptWorker.InsertAttempt(newAttempt);

        public async Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue, ObjectId? memberId = null)
        {
            if (memberId == null)
                memberId = _workContext.MemberId;

            await _cache.Del($"MemberActiveAttempt:{memberId}");

            return await AttemptWorker.SetProperty(attemptId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties, ObjectId? memberId = null)
        {
            if (memberId == null)
                memberId = _workContext.MemberId;

            await _cache.Del($"MemberActiveAttempt:{memberId}");

            return await AttemptWorker.SetProperties(attemptId, properties);
        }
    }
}