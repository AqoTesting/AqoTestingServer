using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class TestRepository : ITestRepository
    {
        
        public async Task<Test> GetTestById(ObjectId testId) =>
            await Task.Run(() => TestWorker.GetTestById(testId));

        public async Task<Test[]> GetTestsByRoomId(ObjectId roomId) =>
            await Task.Run(() => TestWorker.GetTestsByRoomId(roomId));
    }
}