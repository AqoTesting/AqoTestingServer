using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.Interfaces;

namespace AqoTesting.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        public TestRepository()
        {
        }

        public async Task<TestsDB_TestDTO> GetTestById(ObjectId testId) =>
            await TestWorker.GetTestById(testId);

        public async Task<TestsDB_TestDTO[]> GetTestsByRoomId(ObjectId roomId) =>
            await TestWorker.GetTestsByRoomId(roomId);


        public async Task<ObjectId> InsertTest(TestsDB_TestDTO newTest) =>
            await TestWorker.InsertTest(newTest);

        public async Task ReplaceTest(TestsDB_TestDTO updatedTest) =>
            await TestWorker.Replace(updatedTest);


        public async Task<bool> SetProperty(ObjectId testId, string propertyName, object newPropertyValue) =>
            await TestWorker.SetProperty(testId, propertyName, newPropertyValue);

        public async Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties) =>
            await TestWorker.SetProperties(testId, properties);


        public async Task<bool> DeleteTest(ObjectId testId) =>
            await TestWorker.Delete(testId);

        public async Task<long> DeleteTestsByRoomId(ObjectId roomId) =>
            await TestWorker.DeleteTestsByRoomId(roomId);
    }
}