using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using MongoDB.Driver;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class TestWorker
    {
        #region IO
        public static async Task<TestsDB_Test_DTO> GetTestById(ObjectId testId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var test = await MongoController.TestCollection.Find(filter).SingleOrDefaultAsync();

            return test;
        }

        public static async Task<TestsDB_Test_DTO[]> GetTestsByRoomId(ObjectId roomId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("RoomId", roomId);
            var tests = await MongoController.TestCollection.Find(filter).ToListAsync();

            return tests.ToArray();
        }

        public static async Task<ObjectId> InsertTest(TestsDB_Test_DTO test)
        {
            await MongoController.TestCollection.InsertOneAsync(test);

            return test.Id;
        }

        public static async Task<bool> Replace(TestsDB_Test_DTO updatedTest)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", updatedTest.Id);

            return (await MongoController.TestCollection.ReplaceOneAsync(filter, updatedTest)).MatchedCount == 1;
        }

        public static async Task<bool> Delete(ObjectId testId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            return (await MongoController.TestCollection.DeleteOneAsync(filter)).DeletedCount == 1;
        }
        #endregion

        #region Properties
        public static async Task<bool> SetProperty(ObjectId testId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var updates = new List<UpdateDefinition<TestsDB_Test_DTO>>();
            var update = Builders<TestsDB_Test_DTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.TestCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
