using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AqoTesting.Domain.Workers
{
    public static class TestWorker
    {
        #region IO
        public static Test GetTestById(ObjectId testId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var test = MongoController.TestCollection.Find(filter).SingleOrDefault();
            return test;
        }

        public static Test[] GetTestsByIds(ObjectId[] тестИдс)
        {
            var filter = Builders<Test>.Filter.In("Id", тестИдс);
            var tests = MongoController.TestCollection.Find(filter).ToEnumerable();
            return tests.ToArray();
        }

        public static ObjectId InsertTest(Test test)
        {
            MongoController.TestCollection.InsertOne(test);
            return test.Id;
        }

        public static ObjectId[] InsertTests(Test[] tests)
        {
            MongoController.TestCollection.InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach (var test in tests)
                TestsIds.Add(test.Id);

            return TestsIds.ToArray();
        }

        public static bool DeleteTestById(ObjectId testId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var isDeleteSuccessful = MongoController.TestCollection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Props

        public static void SetTestTitle(ObjectId testId, string newTitle)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Title", newTitle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetTitle(this Test test, string newTitle) => SetTestTitle(test.Id, newTitle);

        public static void SetTestUserId(ObjectId testId, ObjectId newUserId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("UserId", newUserId);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetUserId(this Test test, ObjectId newUserId) => SetTestUserId(test.Id, newUserId);

        public static void SetTestIsActive(ObjectId testId, bool newIsActive)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("IsActive", newIsActive);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetIsActive(this Test test, bool newIsActive) => SetTestIsActive(test.Id, newIsActive);

        public static void SetTestSections(ObjectId testId, Section[] newSections)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Sections", newSections);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetSections(this Test test, Section[] newSections) => SetTestSections(test.Id, newSections);

        public static void SetTestActivationDate(ObjectId testId, DateTime newActivationDate)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("ActivationDate", newActivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetActivationDate(this Test test, DateTime newActivationDate) => SetTestActivationDate(test.Id, newActivationDate);

        public static void SetTestDeactivationDate(ObjectId testId, DateTime newDeactivationDate)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("DeactivationDate", newDeactivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetDeactivationDate(this Test test, DateTime newDeactivationDate) => SetTestDeactivationDate(test.Id, newDeactivationDate);

        public static void SetTestShuffle(ObjectId testId, bool newShuffle)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Shuffle", newShuffle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        public static void SetShuffle(this Test test, bool newShuffle) => SetTestShuffle(test.Id, newShuffle);

        #endregion
    }
}
