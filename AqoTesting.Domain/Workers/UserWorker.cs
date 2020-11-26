using AqoTesting.Domain.Controllers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using AqoTesting.Shared.DTOs.DB.Users;
using System.Threading.Tasks;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
namespace AqoTesting.Domain.Workers
{
    public static class UserWorker
    {
        #region Room
        public static async Task<RoomsDB_Room_DTO?> GetUserRoom(ObjectId UserId, ObjectId RoomId) //а нужно ли?
        {
            var idFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("UserId", UserId);
            var filter = idFilter & ownerFilter;
            var room = await MongoController.RoomCollection.Find(filter).SingleOrDefaultAsync();

            return room;
        }

        public static async Task<RoomsDB_Room_DTO[]> GetUserRooms(ObjectId UserId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("UserId", UserId);
            var rooms = await MongoController.RoomCollection.Find(filter).ToListAsync();

            return rooms.ToArray();
        }

        public static async Task<RoomsDB_Room_DTO[]> GetRooms(this UsersDB_User_DTO user) => await GetUserRooms(user.Id);

        public static async Task<ObjectId[]> GetUserRoomsId(ObjectId UserId)
        {
            var filter = Builders<RoomsDB_Room_DTO>.Filter.Eq("UserId", UserId);
            var rooms = await MongoController.RoomCollection.Find(filter).Project<RoomsDB_Room_DTO>("{ _id:1}").ToListAsync();

            return rooms.Select(room => room.Id).ToArray();
        }

        public static async Task<bool> IsUserOwner(ObjectId UserId, ObjectId RoomId)
        {
            var idFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("Id", RoomId);
            var ownerFilter = Builders<RoomsDB_Room_DTO>.Filter.Eq("UserId", UserId);
            var filter = idFilter & ownerFilter;
            var isOwner = (await MongoController.RoomCollection.Find(filter).CountDocumentsAsync()) == 1;

            return isOwner;
        }

        public static async Task<bool> IsOwner(this UsersDB_User_DTO user, ObjectId RoomId) => await IsUserOwner(user.Id, RoomId);

        #endregion

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

        public static async Task<ObjectId[]> InsertUsers(UsersDB_User_DTO[] users)
        {
            await MongoController.UserCollection.InsertManyAsync(users);

            return users.Select(user => user.Id).ToArray();
        }

        public static async Task<bool> DeleteUserById(ObjectId userId)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var isDeleteSuccessful = (await MongoController.UserCollection.DeleteOneAsync(filter)).DeletedCount == 1;

            return isDeleteSuccessful;
        }

        public static async Task<bool> DeleteFromDB(this UsersDB_User_DTO user) => await DeleteUserById(user.Id);

        public static async Task<bool> DeleteUserByLogin(string userLogin)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Login", userLogin);
            var isDeleteSuccessful = (await MongoController.UserCollection.DeleteOneAsync(filter)).DeletedCount == 1;

            return isDeleteSuccessful;
        }

        #endregion

        #region Props

        public static async Task<bool> SetUserLogin(ObjectId userId, string newLogin)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Login", newLogin);
            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetLogin(this UsersDB_User_DTO user, string newLogin)
        {
            user.Login = newLogin;
            return await SetUserLogin(user.Id, newLogin);
        }

        public static async Task<bool> SetUserEmail(ObjectId userId, string newEmail)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Email", newEmail);
            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetEmail(this UsersDB_User_DTO user, string newEmail)
        {
            user.Email = newEmail;
            return await SetUserEmail(user.Id, newEmail);
        }

        public static async Task<bool> SetUserPasswordHash(ObjectId userId, byte[] newPasswordHash)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("PasswordHash", newPasswordHash);
            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetPasswordHash(this UsersDB_User_DTO user, byte[] newPasswordHash)
        {
            user.PasswordHash = newPasswordHash;
            return await SetUserPasswordHash(user.Id, newPasswordHash);
        }

        public static async Task<bool> SetUserName(ObjectId userId, string newName)
        {
            var filter = Builders<UsersDB_User_DTO>.Filter.Eq("Id", userId);
            var update = Builders<UsersDB_User_DTO>.Update.Set("Name", newName);
            return (await MongoController.UserCollection.UpdateOneAsync(filter, update)).MatchedCount == 1;
        }

        public static async Task<bool> SetName(this UsersDB_User_DTO user, string newName)
        {
            user.Name = newName;
            return await SetUserName(user.Id, newName);
        }

        #endregion
    }
}
