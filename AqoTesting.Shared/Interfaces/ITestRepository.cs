using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestRepository
    {
        Task<Test> GetTestById(ObjectId testId);

        Task<Test[]> GetTestsByRoomId(ObjectId roomId);
    }
}
