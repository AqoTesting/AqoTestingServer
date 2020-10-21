using AqoTesting.Shared.DTOs.BD;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.DTOs.BD.Tests;
using AqoTesting.Shared.DTOs.BD.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoIOController
    {
        #region GET
        public static Room GetRoomById(ObjectId roomId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = collection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room GetRoomByDomain(string domain) {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("Domain", domain);
            var room = collection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room[] GetRoomsByOwnerId(ObjectId ownerId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("OwnerId", ownerId);
            var rooms = collection.Find(filter).ToList<Room>();

            return rooms.ToArray();
        }

        public static Test GetTestById(ObjectId testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            var test = collection.Find(filter).SingleOrDefault();
            return test;
        }

        public static User GetUserById(ObjectId userId)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var user = collection.Find(filter).SingleOrDefault();
            return user;
        }

        public static User GetUserByAuthData(string login, byte[] passwordHash)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var loginFilter = Builders<User>.Filter.Eq("Email", login) | Builders<User>.Filter.Eq("Login", login);
            var PasswordFilter = Builders<User>.Filter.Eq("PasswordHash", passwordHash);
            var filter = loginFilter & PasswordFilter;
            var user = collection.Find(filter).SingleOrDefault();
            return user;
        }

        public static User GetUserByLogin(string login) {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("Login", login);
            var user = collection.Find(filter).SingleOrDefault();
            return user;
        }

        public static User GetUserByEmail(string login)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("Email", login);
            var user = collection.Find(filter).SingleOrDefault();
            return user;
        }

        #endregion

        #region INSERT
        public static ObjectId InsertRoom(Room room)
        {
            MongoController.mainDatabase.GetCollection<Room>("rooms").InsertOne(room);
            return room.Id;
        }

        public static ObjectId[] InsertRooms(Room[] rooms)
        {
            MongoController.mainDatabase.GetCollection<Room>("rooms").InsertMany(rooms);
            var RoomsIds = new List<ObjectId>();
            foreach (var room in rooms)
                RoomsIds.Add(room.Id);

            return RoomsIds.ToArray();
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

        public static ObjectId InsertUser(User user)
        {
            MongoController.mainDatabase.GetCollection<User>("users").InsertOne(user);
            return user.Id;
        }

        public static ObjectId[] InsertUsers(User[] users)
        {
            MongoController.mainDatabase.GetCollection<User>("users").InsertMany(users);
            var UsersIds = new List<ObjectId>();
            foreach (var user in users)
                UsersIds.Add(user.Id);

            return UsersIds.ToArray();
        }
        #endregion

        #region DELETE
        public static void DeleteRoomById(ObjectId roomId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("Id", roomId);
            collection.DeleteOne(filter);
        }

        public static void DeleteTestById(ObjectId testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var filter = Builders<Test>.Filter.Eq("Id", testId);
            collection.DeleteOne(filter);
        }

        public static void DeleteUserById(ObjectId userId)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("Id", userId);
            collection.DeleteOne(filter);
        }

        #endregion
    }
}
