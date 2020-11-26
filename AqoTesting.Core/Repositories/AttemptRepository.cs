using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        ICacheRepository _cache;
        public AttemptRepository(ICacheRepository cache)
        {
            _cache = cache;
        }

        public async Task<AttemptsDB_Attempt_DTO> GetAttemptById(ObjectId attemptId) =>
            await AttemptWorker.GetAttemptById(attemptId);
    }
}