using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        ICacheRepository _cache;
        public TestRepository(ICacheRepository cache)
        {
            _cache = cache;
        }

        public async Task<TestsDB_Test_DTO> GetTestById(ObjectId testId) =>
            await _cache.Get<TestsDB_Test_DTO>($"Test:{testId}", () => TestWorker.GetTestById(testId));

        public async Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId) =>
            await Task.Run(() => TestWorker.GetTestsByRoomId(roomId));
    }
}