using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Driver;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoController
    {
        public static string? connectionUri;
        public static MongoClient? client;
        public static IMongoDatabase mainDatabase;

        public static MongoClient? ConnectToDB(string? username, string? password, string host, ushort? port, string? database, string? options)
        {
            connectionUri = "mongodb://" + (username != null ? $"{username}:{password}@" : "") +
                host + (port != null ? $":{port}" : "") +
                (database != null || options != null ? "/" : "") +
                (database != null ? $"{database}" : "") +
                (options != null ? $"?options" : "");

            client = new MongoClient(connectionUri);
            if (client != null && database != null) mainDatabase = client.GetDatabase(database);

            return client;
        }

        public static IMongoCollection<Room> GetRoomsCollection()
        {
            return mainDatabase.GetCollection<Room>("rooms");
        }

        public static IMongoCollection<Test> GetTestsCollection()
        {
            return mainDatabase.GetCollection<Test>("tests");
        }

        public static IMongoCollection<User> GetUsersCollection()
        {
            return mainDatabase.GetCollection<User>("users");
        }
    }
}
