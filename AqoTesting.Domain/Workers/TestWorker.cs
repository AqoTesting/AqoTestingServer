using System;
using System.Collections.Generic;
using System.Linq;
using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Tests;
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
        public static TestsDB_Test_DTO GetTestById(ObjectId testId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var test = MongoController.TestCollection.Find(filter).SingleOrDefault();
            return test;
        }
        /// <summary>
        /// Получение тестов по id комнаты
        /// </summary>
        /// <param name="testIds"></param>
        /// <returns>список тестов</returns>
        public static TestsDB_Test_DTO[] GetTestsByRoomId(ObjectId roomId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("RoomId", roomId);
            var tests = MongoController.TestCollection.Find(filter).ToEnumerable();
            return tests.ToArray();
        }
        /// <summary>
        /// Вставка теста в базу
        /// </summary>
        /// <param name="test"></param>
        /// <returns>id теста</returns>
        public static ObjectId InsertTest(TestsDB_Test_DTO test)
        {
            MongoController.TestCollection.InsertOne(test);
            return test.Id;
        }
        /// <summary>
        /// Вставка списка тестов в базу
        /// </summary>
        /// <param name="tests"></param>
        /// <returns>список id тестов</returns>
        public static ObjectId[] InsertTests(TestsDB_Test_DTO[] tests)
        {
            MongoController.TestCollection.InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach(var test in tests)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Title", newTitle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка заголовка теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newTitle"></param>
        public static void SetTitle(this TestsDB_Test_DTO test, string newTitle)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("OwnerId", newOwnerId);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка создателя теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newOwnerId"></param>
        public static void SetOwnerId(this TestsDB_Test_DTO test, ObjectId newOwnerId)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("IsActive", newIsActive);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка активности теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newIsActive"></param>
        public static void SetIsActive(this TestsDB_Test_DTO test, bool newIsActive)
        {
            test.IsActive = newIsActive;
            SetTestIsActive(test.Id, newIsActive);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newSections"></param>
        public static void SetTestSections(ObjectId testId, TestsDB_Section_DTO[] newSections)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Sections", newSections);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newSections"></param>
        public static void SetSections(this TestsDB_Test_DTO test, TestsDB_Section_DTO[] newSections)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("ActivationDate", newActivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты активации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newActivationDate"></param>
        public static void SetActivationDate(this TestsDB_Test_DTO test, DateTime newActivationDate)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("DeactivationDate", newDeactivationDate);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты деактивации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newDeactivationDate"></param>
        public static void SetDeactivationDate(this TestsDB_Test_DTO test, DateTime newDeactivationDate)
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
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Shuffle", newShuffle);
            MongoController.TestCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка Перемешивания
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newShuffle"></param>
        public static void SetShuffle(this TestsDB_Test_DTO test, bool newShuffle)
        {
            test.Shuffle = newShuffle;
            SetTestShuffle(test.Id, newShuffle);
        }

        #endregion
    }
}
