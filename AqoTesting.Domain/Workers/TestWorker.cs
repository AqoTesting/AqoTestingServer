using System;
using System.Collections.Generic;
using System.Linq;
using AqoTesting.Domain.Controllers;
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
            var test = MongoController.TestCollection?.Find(filter).SingleOrDefault();
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
            var tests = MongoController.TestCollection?.Find(filter).ToEnumerable();
            return tests.ToArray();
        }
        /// <summary>
        /// Вставка теста в базу
        /// </summary>
        /// <param name="test"></param>
        /// <returns>id теста</returns>
        public static ObjectId InsertTest(TestsDB_Test_DTO test)
        {
            MongoController.TestCollection?.InsertOne(test);
            return test.Id;
        }
        /// <summary>
        /// Вставка списка тестов в базу
        /// </summary>
        /// <param name="tests"></param>
        /// <returns>список id тестов</returns>
        public static ObjectId[] InsertTests(TestsDB_Test_DTO[] tests)
        {
            MongoController.TestCollection?.InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach(var test in tests)
                TestsIds.Add(test.Id);

            return TestsIds.ToArray();
        }

        public static void ReplaceTest(TestsDB_Test_DTO updatedTest)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", updatedTest.Id);
            MongoController.TestCollection?.ReplaceOne(filter, updatedTest);
        }

        /// <summary>
        /// Удаление теста по id
        /// </summary>
        /// <param name="testId"></param>
        /// <returns>успех</returns>
        public static bool DeleteTestById(ObjectId testId)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var isDeleteSuccessful = MongoController.TestCollection?.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }
        #endregion

        #region Props
        /// <summary>
        /// Установка заголовка теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestTitle(ObjectId testId, string newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Title", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка заголовка теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetTitle(this TestsDB_Test_DTO test, string newValue)
        {
            test.Title = newValue;
            SetTestTitle(test.Id, newValue);
        }
        /// <summary>
        /// Установка создателя теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestOwnerId(ObjectId testId, ObjectId newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("OwnerId", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка создателя теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetOwnerId(this TestsDB_Test_DTO test, ObjectId newValue)
        {
            test.OwnerId = newValue;
            SetTestOwnerId(test.Id, newValue);
        }
        /// <summary>
        /// Установка активности теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestIsActive(ObjectId testId, bool newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("IsActive", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка активности теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetIsActive(this TestsDB_Test_DTO test, bool newValue)
        {
            test.IsActive = newValue;
            SetTestIsActive(test.Id, newValue);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestSections(ObjectId testId, Dictionary<string, TestsDB_Section_DTO> newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Sections", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка секций теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetSections(this TestsDB_Test_DTO test, Dictionary<string, TestsDB_Section_DTO> newValue)
        {
            test.Sections = newValue;
            SetTestSections(test.Id, newValue);
        }
        /// <summary>
        /// Установка даты активации теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestActivationDate(ObjectId testId, DateTime newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("ActivationDate", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты активации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetActivationDate(this TestsDB_Test_DTO test, DateTime newValue)
        {
            test.ActivationDate = newValue;
            SetTestActivationDate(test.Id, newValue);
        }
        /// <summary>
        /// Установка даты деактивации теста
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestDeactivationDate(ObjectId testId, DateTime newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("DeactivationDate", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка даты деактивации теста
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetDeactivationDate(this TestsDB_Test_DTO test, DateTime newValue)
        {
            test.DeactivationDate = newValue;
            SetTestDeactivationDate(test.Id, newValue);
        }
        /// <summary>
        /// Установка перемешивания
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="newValue"></param>
        public static void SetTestShuffle(ObjectId testId, bool newValue)
        {
            var filter = Builders<TestsDB_Test_DTO>.Filter.Eq("Id", testId);
            var update = Builders<TestsDB_Test_DTO>.Update.Set("Shuffle", newValue);
            MongoController.TestCollection?.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка Перемешивания
        /// </summary>
        /// <param name="test"></param>
        /// <param name="newValue"></param>
        public static void SetShuffle(this TestsDB_Test_DTO test, bool newValue)
        {
            test.Shuffle = newValue;
            SetTestShuffle(test.Id, newValue);
        }

        #endregion
    }
}
