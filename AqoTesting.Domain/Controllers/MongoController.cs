using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using MongoDB.Driver;
using System;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoController
    {
        public static string? connectionString;
        public static MongoClient? client;
        public static IMongoDatabase? mainDatabase;
        public static IMongoCollection<RoomsDB_RoomDTO>? RoomCollection;
        public static IMongoCollection<TestsDB_TestDTO>? TestCollection;
        public static IMongoCollection<UsersDB_UserDTO>? UserCollection;
        public static IMongoCollection<MembersDB_MemberDTO>? MemberCollection;
        public static IMongoCollection<AttemptsDB_AttemptDTO>? AttemptCollection;


        public static MongoClient? ConnectToDB(string? username, string? password, string host, ushort? port, string? defaultauthdb, string? options)
        {
            connectionString = "mongodb://" + username + (username != "" && password != "" ? ":" : "") + password +
                host + (port != 0 ? $":{port}" : "") +
                (defaultauthdb != "" || options != "" ? "/" : "") +
                (defaultauthdb != "" ? $"{defaultauthdb}" : "") +
                (options != "" ? $"?{options}" : "");

            client = new MongoClient(connectionString);
            if(client != null && defaultauthdb != null)
            {
                if(defaultauthdb != null)
                    mainDatabase = client.GetDatabase(defaultauthdb);
                PreInitCollections();
            }

            Console.WriteLine("Connected to MongoDB");

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

        public static IMongoCollection<RoomsDB_RoomDTO> GetRoomsCollection() => mainDatabase.GetCollection<RoomsDB_RoomDTO>("rooms");
        public static IMongoCollection<TestsDB_TestDTO> GetTestsCollection() => mainDatabase.GetCollection<TestsDB_TestDTO>("tests");
        public static IMongoCollection<UsersDB_UserDTO> GetUsersCollection() => mainDatabase.GetCollection<UsersDB_UserDTO>("users");
        public static IMongoCollection<MembersDB_MemberDTO> GetMembersCollection() => mainDatabase.GetCollection<MembersDB_MemberDTO>("members");
        public static IMongoCollection<AttemptsDB_AttemptDTO> GetAttemptsCollection() => mainDatabase.GetCollection<AttemptsDB_AttemptDTO>("attempts");
    }
}
