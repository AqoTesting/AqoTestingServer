using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestRepository
    {
        Task<TestsDB_Test_DTO> GetTestById(ObjectId testId);

        Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId);

        Task<ObjectId> InsertTest(TestsDB_Test_DTO newTest);

        Task ReplaceTest(TestsDB_Test_DTO updatedTest);

        Task<bool> SetProperty(ObjectId testId, string propertyName, object newPropertyValue);
        Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties);

        Task<bool> DeleteTest(ObjectId testId);
    }
}
