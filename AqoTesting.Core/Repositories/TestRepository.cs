using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        public async Task<Test[]> GetTestsByIds(ObjectId[] testIds) =>
            await Task.Run(() => TestWorker.GetTestsByIds(testIds));

        public async Task<Test> GetTestById(ObjectId testId) =>
            await Task.Run(() => TestWorker.GetTestById(testId));
    }
}