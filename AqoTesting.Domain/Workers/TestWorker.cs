using System;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task<ObjectId[]> InsertTests(TestsDB_Test_DTO[] tests)
        {
            await MongoController.TestCollection.InsertManyAsync(tests);
            var TestsIds = new List<ObjectId>();
            foreach(var test in tests)
                TestsIds.Add(test.Id);

            return TestsIds.ToArray();
        }

        public static async Task<bool> ReplaceTest(TestsDB_Test_DTO updatedTest)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", updatedTest.Id);
            return (await MongoController.TestCollection.ReplaceOneAsync(filter, updatedTest)).MatchedCount == 1;
        }

        public static async Task<bool> DeleteTestById(ObjectId testId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            return (await MongoController.TestCollection.DeleteOneAsync(filter)).DeletedCount == 1;
        }
        #endregion

        #region Props

        public static async Task<bool> SetTestTitle(ObjectId testId, string newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Title", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetTitle(this TestsDB_Test_DTO test, string newValue)
        {
            test.Title = newValue;
            return await SetTestTitle(test.Id, newValue);
        }

        public static async Task<bool> SetTestUserId(ObjectId testId, ObjectId newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("UserId", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetUserId(this TestsDB_Test_DTO test, ObjectId newValue)
        {
            test.UserId = newValue;
            return await SetTestUserId(test.Id, newValue);
        }

        public static async Task<bool> SetTestIsActive(ObjectId testId, bool newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("IsActive", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetIsActive(this TestsDB_Test_DTO test, bool newValue)
        {
            test.IsActive = newValue;
            return await SetTestIsActive(test.Id, newValue);
        }

        public static async Task<bool> SetTestSections(ObjectId testId, Dictionary<string, TestsDB_Section_DTO> newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Sections", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetSections(this TestsDB_Test_DTO test, Dictionary<string, TestsDB_Section_DTO> newValue)
        {
            test.Sections = newValue;
            return await SetTestSections(test.Id, newValue);
        }

        public static async Task<bool> SetTestActivationDate(ObjectId testId, DateTime newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("ActivationDate", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetActivationDate(this TestsDB_Test_DTO test, DateTime newValue)
        {
            test.ActivationDate = newValue;
            return await SetTestActivationDate(test.Id, newValue);
        }

        public static async Task<bool> SetTestDeactivationDate(ObjectId testId, DateTime newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("DeactivationDate", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetDeactivationDate(this TestsDB_Test_DTO test, DateTime newValue)
        {
            test.DeactivationDate = newValue;
            return await SetTestDeactivationDate(test.Id, newValue);
        }

        public static async Task<bool> SetTestShuffle(ObjectId testId, bool newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Shuffle", newValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetShuffle(this TestsDB_Test_DTO test, bool newValue)
        {
            test.Shuffle = newValue;
            return await SetTestShuffle(test.Id, newValue);
        }

        public static async Task<bool> SetProperty(ObjectId testId, string propName, object propValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set(propName, propValue);
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId testId, Dictionary<string, object> properties)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var updates = new List<UpdateDefinition<TestsDB_Test_DTO>>();
            var update = Builders<TestsDB_Test_DTO>.Update;
            foreach (KeyValuePair<string, object> kvp in propertys)
            {
                updates.Add(update.Set(kvp.Key, kvp.Value));
            }
            return (await MongoController.TestCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
