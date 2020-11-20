using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using AqoTesting.Shared.DTOs.DB.Users;

namespace AqoTesting.Domain.Workers
{
    public static class UserWorker
    {
        #region Room
        /// <summary>
        /// Получение комнаты, если юзер является её владельцем
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoomId"></param>
        /// <returns>Комната или null</returns>
        public static RoomsDB_Room_DTO GetUserRoom(ObjectId UserId, ObjectId RoomId) //а нужно ли?
        {
            var idFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("OwnerId", UserId);
            var filter = idFilter & ownerFilter;
            var room = MongoController.RoomCollection.Find(filter).SingleOrDefault();

            return room;
        }
        /// <summary>
        /// Получение комнат, которыми владеет юзер
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>Список комнат</returns>
        public static RoomsDB_Room_DTO[] GetUserRooms(ObjectId UserId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("OwnerId", UserId);
            var rooms = MongoController.RoomCollection.Find(filter).ToList();

            return rooms.ToArray();
        }
        /// <summary>
        /// Получение комнат, которыми владеет юзер
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Список комнат</returns>
        public static RoomsDB_Room_DTO[] GetRooms(this UsersDB_User_DTO user) => GetUserRooms(user.Id);
        /// <summary>
        /// Получение id комнат, которыми владеет юзер
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>Список id комнат</returns>
        public static ObjectId[] GetUserRoomsId(ObjectId UserId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("OwnerId", UserId);
            var rooms = MongoController.RoomCollection.Find(filter).Project<RoomsDB_Room_DTO>("{ _id:1}").ToList();

            return rooms.Select(room => room.Id).ToArray();
        }
        /// <summary>
        /// Определяет, является ли юзер владельцем
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoomId"></param>
        /// <returns>true если юзер владелец комнаты, инача false</returns>
        public static bool IsUserOwner(ObjectId UserId, ObjectId RoomId)
        {
            var idFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("OwnerId", UserId);
            var filter = idFilter & ownerFilter;
            var isOwner = MongoController.RoomCollection.Find(filter).CountDocuments() == 1;

            return isOwner;
        }
        /// <summary>
        /// Определяет, является ли юзер владельцем
        /// </summary>
        /// <param name="user"></param>
        /// <param name="RoomId"></param>
        /// <returns>true если юзер владелец комнаты, инача false</returns>
        public static bool IsOwner(this UsersDB_User_DTO user, ObjectId RoomId) => IsUserOwner(user.Id, RoomId);

        #endregion

        #region IO
        /// <summary>
        /// Получение юзера по id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Юзер или null</returns>
        public static UsersDB_User_DTO GetUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var user = MongoController.UserCollection.Find(filter).SingleOrDefault();

            return user;
        }
        /// <summary>
        /// Получение юзера по данным авторизации
        /// </summary>
        /// <param name="login"></param>
        /// <param name="passwordHash"></param>
        /// <returns>Юзер или null</returns>
        public static UsersDB_User_DTO GetUserByAuthData(string login, byte[] passwordHash)
        {
            var loginFilter = Builders<UsersDB_User_DTO>.Filter.Eq("Email", login) | Builders<UsersDB_User_DTO>.Filter.Eq("Login", login);
            var passwordFilter = Builders<UsersDB_User_DTO>.Filter.Eq("PasswordHash", passwordHash);
            var filter = loginFilter & passwordFilter;
            var user = MongoController.UserCollection.Find(filter).SingleOrDefault();

            return user;
        }
        /// <summary>
        /// Получение юзера по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Юзер или null</returns>
        public static UsersDB_User_DTO GetUserByLogin(string login)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Login", login);
            var user = MongoController.UserCollection.Find(filter).SingleOrDefault();

            return user;
        }
        /// <summary>
        /// Получение юзера по почте
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>Юзер или null</returns>
        public static UsersDB_User_DTO GetUserByEmail(string Email)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Email", Email);
            var user = MongoController.UserCollection.Find(filter).SingleOrDefault();

            return user;
        }
        /// <summary>
        /// Вставка юзера в базу
        /// </summary>
        /// <param name="user"></param>
        /// <returns>id юзера в базе</returns>
        public static ObjectId InsertUser(UsersDB_User_DTO user)
        {
            MongoController.UserCollection.InsertOne(user);

            return user.Id;
        }
        /// <summary>
        /// Вставка списка юзеров в базу
        /// </summary>
        /// <param name="users"></param>
        /// <returns>список id юзеров в базе</returns>
        public static ObjectId[] InsertUsers(UsersDB_User_DTO[] users)
        {
            MongoController.UserCollection.InsertMany(users);

            return users.Select(user => user.Id).ToArray();
        }
        /// <summary>
        /// Удаление юзера по id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Успех</returns>
        public static bool DeleteUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var isDeleteSuccessful = MongoController.UserCollection.DeleteOne(filter).DeletedCount == 1;

            return isDeleteSuccessful;
        }
        /// <summary>
        /// Удаление юзера из базы
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Успех</returns>
        public static bool DeleteFromDB(this UsersDB_User_DTO user) => DeleteUserById(user.Id);
        /// <summary>
        /// Удаление юзера по логину
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns>Успех</returns>
        public static bool DeleteUserByLogin(string userLogin)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Login", userLogin);
            var isDeleteSuccessful = MongoController.UserCollection.DeleteOne(filter).DeletedCount == 1;

            return isDeleteSuccessful;
        }

        #endregion

        #region Props
        /// <summary>
        /// Установка логина юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newLogin"></param>
        public static void SetUserLogin(ObjectId userId, string newLogin)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Login", newLogin);
            MongoController.UserCollection.UpdateOne(filter, update);
        }/// <summary>
         /// Установка логина юзера
         /// </summary>
         /// <param name="user"></param>
         /// <param name="newLogin"></param>
        public static void SetLogin(this UsersDB_User_DTO user, string newLogin)
        {
            user.Login = newLogin;
            SetUserLogin(user.Id, newLogin);
        }
        /// <summary>
        /// Установка почты юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newEmail"></param>
        public static void SetUserEmail(ObjectId userId, string newEmail)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Email", newEmail);
            MongoController.UserCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка почты юзера
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newEmail"></param>
        public static void SetEmail(this UsersDB_User_DTO user, string newEmail)
        {
            user.Email = newEmail;
            SetUserEmail(user.Id, newEmail);
        }
        /// <summary>
        /// Установка хеша пароля юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPasswordHash"></param>
        public static void SetUserPasswordHash(ObjectId userId, byte[] newPasswordHash)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("PasswordHash", newPasswordHash);
            MongoController.UserCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка хеша пароля юзера
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPasswordHash"></param>
        public static void SetPasswordHash(this UsersDB_User_DTO user, byte[] newPasswordHash)
        {
            user.PasswordHash = newPasswordHash;
            SetUserPasswordHash(user.Id, newPasswordHash);
        }
        /// <summary>
        /// Установка имени юзера
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newName"></param>
        public static void SetUserName(ObjectId userId, string newName)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Name", newName);
            MongoController.UserCollection.UpdateOne(filter, update);
        }
        /// <summary>
        /// Установка имени юзера
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newName"></param>
        public static void SetName(this UsersDB_User_DTO user, string newName)
        {
            user.Name = newName;
            SetUserName(user.Id, newName);
        }

        #endregion
    }
}
