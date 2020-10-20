using AqoTesting.Shared.DTOs.BD;
using AqoTesting.Shared.DTOs.BD.Tests;
using AqoTesting.Shared.DTOs.BD.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoIOController
    {
        public static Test GetTestById(ObjectId testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var test = collection.Find(Builders<Test>.Filter.Eq("Id", testId)).SingleOrDefault();
            return test;
        }

        public static Test GetTestById(string testId)
        {
            return GetTestById(ObjectId.Parse(testId)); // [Возможная ошибка] System.FormatException
        }

        public static User GetUserById(ObjectId userId)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var user = collection.Find(Builders<User>.Filter.Eq("Id", userId)).SingleOrDefault();
            return user;
        }

        public static User GetUserById(string userId)
        {
            Console.WriteLine(userId);
            return GetUserById(ObjectId.Parse(userId)); // [Возможная ошибка] System.FormatException
        }
    }
}
