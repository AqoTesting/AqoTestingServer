using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AqoTesting.Shared.DTOs.DB.Users;

namespace AqoTesting.Domain.Workers
{
    public static class UserWorker
    {
        #region Room

        public static Room GetUserRoom(ObjectId UserId, ObjectId RoomId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var idFilter = Builders<Room>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var filter = idFilter & ownerFilter;
            var room = collection.Find(filter).SingleOrDefault();
            return room;
        }

        public static Room[] GetUserRooms(ObjectId UserId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var rooms = collection.Find(filter).ToList();
            return rooms.ToArray();
        }

        public static ObjectId[] GetUserRoomsId(ObjectId UserId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var filter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var rooms = collection.Find(filter).Project<Room>("{ _id:1}").ToList();

            return rooms.Select(room => room.Id).ToArray();
        }

        public static bool IsUserOwner(ObjectId UserId, ObjectId RoomId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Room>("rooms");
            var idFilter = Builders<Room>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<Room>.Filter.Eq("OwnerId", UserId);
            var filter = idFilter & ownerFilter;
            var isOwner = collection.Find(filter).CountDocuments() == 1;
            return isOwner;
        }

        #endregion

        #region IO

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
            var passwordFilter = Builders<User>.Filter.Eq("PasswordHash", passwordHash);
            var filter = loginFilter & passwordFilter;
            var user = collection.Find(filter).SingleOrDefault();
            return user;
        }

        public static User GetUserByLogin(string login)
        {
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

        public static ObjectId InsertUser(User user)
        {
            MongoController.mainDatabase.GetCollection<User>("users").InsertOne(user);
            return user.Id;
        }

        public static ObjectId[] InsertUsers(User[] users)
        {
            MongoController.mainDatabase.GetCollection<User>("users").InsertMany(users);

            return users.Select(user => user.Id).ToArray();
        }

        public static bool DeleteUserById(ObjectId userId)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var isDeleteSuccessful = collection.DeleteOne(filter).DeletedCount == 1;
            return isDeleteSuccessful;
        }

        #endregion

        #region props

        public static void SetUserLogin(ObjectId userId, string newLogin)
        {
            var collection = MongoController.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var update = Builders<User>.Update.Set("Login", newLogin);
            collection.UpdateOne(filter, update);
        }

        public static void SetLogin(this User user, string newName) => SetUserLogin(user.Id, newName);

        public static void SetUserEmail(ObjectId userId, string newEmail)
        {
            var collection = MongoController.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var update = Builders<User>.Update.Set("Email", newEmail);
            collection.UpdateOne(filter, update);
        }

        public static void SetEmail(this User user, string newEmail) => SetUserEmail(user.Id, newEmail);

        public static void SetUserPasswordHash(ObjectId userId, byte[] newPasswordHash)
        {
            var collection = MongoController.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var update = Builders<User>.Update.Set("PasswordHash", newPasswordHash);
            collection.UpdateOne(filter, update);
        }

        public static void SetPasswordHash(this User user, byte[] newPasswordHash) => SetUserPasswordHash(user.Id, newPasswordHash);

        public static void SetUserName(ObjectId userId, string newName)
        {
            var collection = MongoController.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq("Id", userId);
            var update = Builders<User>.Update.Set("Name", newName);
            collection.UpdateOne(filter, update);
        }

        public static void SetName(this User user, string newName) => SetUserName(user.Id, newName);

        #endregion
    }
}
