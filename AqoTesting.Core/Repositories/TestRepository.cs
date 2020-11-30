using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        ICacheRepository _redisCache;
        public TestRepository(ICacheRepository cache)
        {
            _redisCache = cache;
        }

        public async Task<TestsDB_TestDTO> GetTestById(ObjectId testId) =>
            //await _redisCache.Get($"Test:{testId}", async () => await TestWorker.GetTestById(testId));
            await TestWorker.GetTestById(testId);

        public async Task<TestsDB_TestDTO[]> GetTestsByRoomId(ObjectId roomId) =>
            await TestWorker.GetTestsByRoomId(roomId);

        public async Task<ObjectId> InsertTest(TestsDB_TestDTO newTest) =>
            await TestWorker.InsertTest(newTest);

        public async Task ReplaceTest(TestsDB_TestDTO updatedTest)
        {
            await TestWorker.Replace(updatedTest);
            await _redisCache.Del($"Test:{updatedTest.Id}");
        }

        public async Task<bool> SetProperty(ObjectId testId, string propertyName, object newPropertyValue)
        {
            await _redisCache.Del($"Test:{testId}");

            return await TestWorker.SetProperty(testId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties)
        {
            await _redisCache.Del($"Test:{testId}");

            return await TestWorker.SetProperties(testId, properties);
        }

        public async Task<bool> DeleteTest(ObjectId testId)
        {
            var response = await TestWorker.Delete(testId);
            await _redisCache.Del($"Test:{testId}");

            return response;
        }

        public async Task<long> DeleteTestsByRoomId(ObjectId roomId)
        {
            (await TestWorker.GetTestsByRoomId(roomId))
                .Select(async test =>
                    await _redisCache.Del($"Test:{test.Id}"));

            return await TestWorker.DeleteTestsByRoomId(roomId);
        }
    }
}