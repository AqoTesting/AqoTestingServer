using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Driver;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoController
    {
        public static string? leshaPidor;
        public static MongoClient? client;
        public static IMongoDatabase? mainDatabase;
        public static IMongoCollection<Room>? RoomCollection;
        public static IMongoCollection<Test>? TestCollection;
        public static IMongoCollection<User>? UserCollection;
        public static IMongoCollection<Member>? MemberCollection;


        public static MongoClient? ConnectToDB(string? username, string? password, string host, ushort? port, string? defaultauthdb, string? options)
        {
            leshaPidor = "mongodb://" + (username != null ? $"{username}:{password}@" : "") +
                host + (port != null ? $":{port}" : "") +
                (defaultauthdb != null || options != null ? "/" : "") +
                (defaultauthdb != null ? $"{defaultauthdb}" : "") +
                (options != null ? $"?options" : "");

            client = new MongoClient(leshaPidor);
            if(client != null && defaultauthdb != null)
            {
                if(defaultauthdb != null)
                    mainDatabase = client.GetDatabase(defaultauthdb);
                PreInitCollections();
            }

            return client;
        }

        private static void PreInitCollections()
        {
            RoomCollection = GetRoomsCollection();
            TestCollection = GetTestsCollection();
            UserCollection = GetUsersCollection();
            MemberCollection = GetMembersCollection();
        }

        //ниже нет защиты, все вызовы должны проводится после успешного коннекта

        public static IMongoCollection<Room> GetRoomsCollection() => mainDatabase.GetCollection<Room>("rooms");
        public static IMongoCollection<Test> GetTestsCollection() => mainDatabase.GetCollection<Test>("tests");
        public static IMongoCollection<User> GetUsersCollection() => mainDatabase.GetCollection<User>("users");
        public static IMongoCollection<Member> GetMembersCollection() => mainDatabase.GetCollection<Member>("members");
    }
}
