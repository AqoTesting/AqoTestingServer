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

        public static async Task<UsersDB_UserDTO> GetUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Eq("Id", userId);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_UserDTO> GetUserByAuthData(string login, byte[] passwordHash)
        {
            var loginFilter = Builders<UsersDB_UserDTO>.Filter.Where(user =>
                user.Login.ToLower() == login || user.Email.ToLower() == login );

            var passwordFilter = Builders<UsersDB_UserDTO>.Filter.Eq("PasswordHash", passwordHash);


            var filter = loginFilter & passwordFilter;
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_UserDTO> GetUserByLogin(string login)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Where(user => user.Login.ToLower() == login);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<UsersDB_UserDTO> GetUserByEmail(string email)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Where(user => user.Email.ToLower() == email);
            var user = await MongoController.UserCollection.Find(filter).SingleOrDefaultAsync();

            return user;
        }

        public static async Task<ObjectId> InsertUser(UsersDB_UserDTO user)
        {
            await MongoController.UserCollection.InsertOneAsync(user);

            return user.Id;
        }

        public static async Task<bool> DeleteUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Eq("Id", userId);
            var isDeleteSuccessful = (await MongoController.UserCollection.DeleteOneAsync(filter)).DeletedCount == 1;

            return isDeleteSuccessful;
        }
        public static async Task<bool> DeleteFromDB(this UsersDB_UserDTO user) => await DeleteUserById(user.Id);
        #endregion

        #region Properties
        public static async Task<bool> SetProperty(ObjectId userId, string propertyName, object newPropertyValue)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_UserDTO>.Update.Set(propertyName, newPropertyValue);

            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetProperties(ObjectId userId, Dictionary<string, object> properties)
        {
            var filter = Builders<UsersDB_UserDTO>.Filter.Eq("Id", userId);
            var updates = new List<UpdateDefinition<UsersDB_UserDTO>>();
            var update = Builders<UsersDB_UserDTO>.Update;
            foreach (KeyValuePair<string, object> property in properties)
                updates.Add(update.Set(property.Key, property.Value));

            return (await MongoController.UserCollection.UpdateOneAsync(filter, update.Combine(updates.ToArray()))).MatchedCount == 1;
        }
        #endregion
    }
}
