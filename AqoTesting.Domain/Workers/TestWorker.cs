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
        /// <summary>
        /// Получение теста по id
        /// </summary>
        /// <param name="testId"></param>
        /// <returns>Тест или null</returns>
        public static Test GetTestById(ObjectId testId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var test = MongoController.TestCollection.Find(filter).SingleOrDefault();
            return test;
        }
        /// <summary>
        /// Получение тестов по списку id
        /// </summary>
        /// <param name="testIds"></param>
        /// <returns>список тестов</returns>
        public static Test[] GetTestsByIds(ObjectId[] testIds)
        {
            var filter = Builders<Test>.Filter.In("Id", testIds);
            var tests = MongoController.TestCollection.Find(filter).ToEnumerable();
            return tests.ToArray();
        }
        /// <summary>
        /// Вставка теста в базу
        /// </summary>
        /// <param name="test"></param>
        /// <returns>id теста</returns>
        public static ObjectId InsertTest(Test test)
        {
            MongoController.TestCollection.InsertOne(test);
            return test.Id;
        }
        /// <summary>
        /// Вставка списка тестов в базу
        /// </summary>
        /// <param name="tests"></param>
        /// <returns>список id тестов</returns>
        public static ObjectId[] InsertTests(Test[] tests)
        {
            MongoController.TestCollection.InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach (var test in tests)
                TestsIds.Add(test.Id);

            return TestsIds.ToArray();
        }
        /// <summary>
        /// Удаление теста по id
        /// </summary>
        /// <param name="testId"></param>
        /// <returns>успех</returns>
        public static bool DeleteTestById(ObjectId testId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var isDeleteSuccessful = MongoController.TestCollection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Props
        /// <summary>
        /// Установка заголовка теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newTitle"></param>
        public static void SetTestTitle(ObjectId testId, string newTitle)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Title", newTitle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка заголовка теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newTitle"></param>
        public static void SetTitle(this Test test, string newTitle)
        {
            test.Title = newTitle;
            SetTestTitle(test.Id, newTitle);
        }
        /// <summary>
        /// Установка создателя теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newOwnerId"></param>
        public static void SetTestOwnerId(ObjectId testId, ObjectId newOwnerId)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("OwnerId", newOwnerId);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка создателя теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newOwnerId"></param>
        public static void SetOwnerId(this Test test, ObjectId newOwnerId)
        {
            test.OwnerId = newOwnerId;
            SetTestOwnerId(test.Id, newOwnerId);
        }
        /// <summary>
        /// Установка активности теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newIsActive"></param>
        public static void SetTestIsActive(ObjectId testId, bool newIsActive)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("IsActive", newIsActive);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка активности теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newIsActive"></param>
        public static void SetIsActive(this Test test, bool newIsActive)
        {
            test.IsActive = newIsActive;
            SetTestIsActive(test.Id, newIsActive);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newSections"></param>
        public static void SetTestSections(ObjectId testId, Section[] newSections)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Sections", newSections);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newSections"></param>
        public static void SetSections(this Test test, Section[] newSections)
        {
            test.Sections = newSections;
            SetTestSections(test.Id, newSections);
        }
        /// <summary>
        /// Установка даты активации теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newActivationDate"></param>
        public static void SetTestActivationDate(ObjectId testId, DateTime newActivationDate)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("ActivationDate", newActivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты активации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newActivationDate"></param>
        public static void SetActivationDate(this Test test, DateTime newActivationDate)
        {
            test.ActivationDate = newActivationDate;
            SetTestActivationDate(test.Id, newActivationDate);
        }
        /// <summary>
        /// Установка даты деактивации теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newDeactivationDate"></param>
        public static void SetTestDeactivationDate(ObjectId testId, DateTime newDeactivationDate)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("DeactivationDate", newDeactivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты деактивации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newDeactivationDate"></param>
        public static void SetDeactivationDate(this Test test, DateTime newDeactivationDate)
        {
            test.DeactivationDate = newDeactivationDate;
            SetTestDeactivationDate(test.Id, newDeactivationDate);
        }
        /// <summary>
        /// Установка перемешивания
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newShuffle"></param>
        public static void SetTestShuffle(ObjectId testId, bool newShuffle)
        {
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var update = Builders<Test>.Update.Set("Shuffle", newShuffle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка Перемешивания
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newShuffle"></param>
        public static void SetShuffle(this Test test, bool newShuffle)
        {
            test.Shuffle = newShuffle;
            SetTestShuffle(test.Id, newShuffle); 
        }

        #endregion
    }
}
