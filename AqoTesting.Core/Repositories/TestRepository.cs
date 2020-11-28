using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
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
            //await _cache.Get($"Test:{testId}", async () => await TestWorker.GetTestById(testId));
            await TestWorker.GetTestById(testId);

        public async Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId) =>
            await TestWorker.GetTestsByRoomId(roomId);

        public async Task<ObjectId> InsertTest(TestsDB_Test_DTO newTest) =>
            await TestWorker.InsertTest(newTest);

        public async Task ReplaceTest(TestsDB_Test_DTO updatedTest)
        {
            await TestWorker.Replace(updatedTest);
            await _cache.Del($"Test:{updatedTest.Id}");
        }

        public async Task<bool> SetProperty(ObjectId testId, string propertyName, object newPropertyValue)
        {
            await _cache.Del($"Test:{testId}");

            return await TestWorker.SetProperty(testId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties)
        {
            await _cache.Del($"Test:{testId}");

            return await TestWorker.SetProperties(testId, properties);
        }

        public async Task<bool> DeleteTest(ObjectId testId)
        {
            var response = await Task.Run(async () => await TestWorker.Delete(testId));
            await _cache.Del($"Test:{testId}");

            return response;
        }
    }
}