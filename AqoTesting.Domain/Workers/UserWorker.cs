using AqoTesting.Domain.Controllers;
using MongoDB.Bson;
using MongoDB.Driver;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Threading.Tasks;
using System.Collections.Generic;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class UserWorker
    {
        #region IO

        public static async Task<UsersDB_User_DTO> GetUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_User_DTO> GetUserByAuthData(string login, byte[] passwordHash)
        {
            var loginFilter = Builders<UsersDB_User_DTO>.Filter.Eq("Email", login) | Builders<UsersDB_User_DTO>.Filter.Eq("Login", login);
            var passwordFilter = Builders<UsersDB_User_DTO>.Filter.Eq("PasswordHash", passwordHash);
            var filter = loginFilter & passwordFilter;
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_User_DTO> GetUserByLogin(string login)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Login", login);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_User_DTO> GetUserByEmail(string Email)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Email", Email);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<ObjectId> InsertUser(UsersDB_User_DTO user)
        {
            await MongoController.UserCollection.InsertOneAsync(user);

            return user.Id;
        }

        public static async Task<bool> DeleteUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var isDeleteSuccessful = (await MongoController.UserCollection.DeleteOneAsync(filter)).DeletedCount == 1;

            return isDeleteSuccessful;
        }
        public static async Task<bool> DeleteFromDB(this UsersDB_User_DTO user) => await DeleteUserById(user.Id);
        #endregion

        #region Properties
        public static async Task<bool> SetProperty(ObjectId userId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId userId, Dictionary<string, object> properties)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var updates = new List<UpdateDefinition<UsersDB_User_DTO>>();
            var update = Builders<UsersDB_User_DTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.UserCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
