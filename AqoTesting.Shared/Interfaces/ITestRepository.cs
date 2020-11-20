using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestRepository
    {
        Task<TestsDB_Test_DTO> GetTestById(ObjectId testId);

        Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId);
    }
}
