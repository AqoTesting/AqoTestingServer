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
            //await _cache.Get($"Test:{testId}", () => TestWorker.GetTestById(testId));
            await Task.Run(() => TestWorker.GetTestById(testId));

        public async Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId) =>
            await Task.Run(() => TestWorker.GetTestsByRoomId(roomId));

        public async Task<ObjectId> InsertTest(TestsDB_Test_DTO newTest) =>
            await Task.Run(() => TestWorker.InsertTest(newTest));

        public async Task ReplaceTest(TestsDB_Test_DTO updatedTest)
        {
            await Task.Run(() => TestWorker.ReplaceTest(updatedTest));
            await _cache.Del($"Test:{updatedTest.Id}");
        }

        public async Task SetSections(ObjectId testId, Dictionary<string, TestsDB_Section_DTO> newValue)
        {
            await Task.Run(() => TestWorker.SetTestSections(testId, newValue));
            await _cache.Del($"Test:{testId}");
        }

        public async Task<bool> DeleteTest(ObjectId testId)
        {
            var response = await Task.Run(() => TestWorker.DeleteTestById(testId));
            await _cache.Del($"Test:{testId}");

            return response;
        }
    }
}