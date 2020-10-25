using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AqoTesting.Domain.Workers
{
    public static class TestWorker
    {
        #region IO
        public static Test GetTestById(ObjectId testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var test = collection.Find(filter).SingleOrDefault();
            return test;
        }

        public static ObjectId InsertTest(Test test)
        {
            MongoController.mainDatabase.GetCollection<Test>("tests").InsertOne(test);
            return test.Id;
        }

        public static ObjectId[] InsertTests(Test[] tests)
        {
            MongoController.mainDatabase.GetCollection<Test>("tests").InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach (var test in tests)
                TestsIds.Add(test.Id);

            return TestsIds.ToArray();
        }

        public static bool DeleteTestById(ObjectId testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var isDeleteSuccessful = collection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region props

        public static void SetTestTitle(ObjectId testId, string newTitle)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Title", newTitle);
            collection.UpdateOne(filter, update);
        }

        public static void SetTitle(this Test test, string newTitle) => SetTestTitle(test.Id, newTitle);

        public static void SetTestUserId(ObjectId testId, ObjectId newUserId)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("UserId", newUserId);
            collection.UpdateOne(filter, update);
        }

        public static void SetUserId(this Test test, ObjectId newUserId) => SetTestUserId(test.Id, newUserId);

        public static void SetTestIsActive(ObjectId testId, bool newIsActive)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("IsActive", newIsActive);
            collection.UpdateOne(filter, update);
        }

        public static void SetIsActive(this Test test, bool newIsActive) => SetTestIsActive(test.Id, newIsActive);

        public static void SetTestSections(ObjectId testId, Section[] newSections)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Sections", newSections);
            collection.UpdateOne(filter, update);
        }

        public static void SetSections(this Test test, Section[] newSections) => SetTestSections(test.Id, newSections);

        public static void SetTestActivationDate(ObjectId testId, DateTime newActivationDate)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("ActivationDate", newActivationDate);
            collection.UpdateOne(filter, update);
        }

        public static void SetActivationDate(this Test test, DateTime newActivationDate) => SetTestActivationDate(test.Id, newActivationDate);

        public static void SetTestDeactivationDate(ObjectId testId, DateTime newDeactivationDate)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("DeactivationDate", newDeactivationDate);
            collection.UpdateOne(filter, update);
        }

        public static void SetDeactivationDate(this Test test, DateTime newDeactivationDate) => SetTestDeactivationDate(test.Id, newDeactivationDate);

        public static void SetTestShuffle(ObjectId testId, bool newShuffle)
        {
            var collection = MongoController.GetTestsCollection();
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Shuffle", newShuffle);
            collection.UpdateOne(filter, update);
        }

        public static void SetShuffle(this Test test, bool newShuffle) => SetTestShuffle(test.Id, newShuffle);

        #endregion
    }
}
