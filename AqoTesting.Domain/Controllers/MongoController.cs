using AqoTesting.Shared.DTOs.DB.Attempts;
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
        public static IMongoCollection<RoomsDB_Room_DTO>? RoomCollection;
        public static IMongoCollection<TestsDB_Test_DTO>? TestCollection;
        public static IMongoCollection<UsersDB_User_DTO>? UserCollection;
        public static IMongoCollection<MembersDB_Member_DTO>? MemberCollection;
        public static IMongoCollection<AttemptsDB_Attempt_DTO>? AttemptCollection;


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
            AttemptCollection = GetAttemptsCollection();
        }

        //ниже нет защиты, все вызовы должны проводится после успешного коннекта

        public static IMongoCollection<RoomsDB_Room_DTO> GetRoomsCollection() => mainDatabase.GetCollection<RoomsDB_Room_DTO>("rooms");
        public static IMongoCollection<TestsDB_Test_DTO> GetTestsCollection() => mainDatabase.GetCollection<TestsDB_Test_DTO>("tests");
        public static IMongoCollection<UsersDB_User_DTO> GetUsersCollection() => mainDatabase.GetCollection<UsersDB_User_DTO>("users");
        public static IMongoCollection<MembersDB_Member_DTO> GetMembersCollection() => mainDatabase.GetCollection<MembersDB_Member_DTO>("members");
        public static IMongoCollection<AttemptsDB_Attempt_DTO> GetAttemptsCollection() => mainDatabase.GetCollection<AttemptsDB_Attempt_DTO>("attempts");
    }
}
