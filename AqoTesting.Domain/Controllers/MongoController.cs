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


        public static MongoClient? ConnectToDB(string? leshaPidor1, string? leshaPidor2, string leshaPidor3, ushort? leshaPidor4, string? leshaPidor5, string? leshaPidor6)
        {
            leshaPidor = "mongodb://" + (leshaPidor1 != null ? $"{leshaPidor1}:{leshaPidor2}@" : "") +
                leshaPidor3 + (leshaPidor4 != null ? $":{leshaPidor4}" : "") +
                (leshaPidor5 != null || leshaPidor6 != null ? "/" : "") +
                (leshaPidor5 != null ? $"{leshaPidor5}" : "") +
                (leshaPidor6 != null ? $"?options" : "");

            client = new MongoClient(leshaPidor);
            if (client != null && leshaPidor5 != null)
            {
                if (leshaPidor5 != null)
                    mainDatabase = client.GetDatabase(leshaPidor5);
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
