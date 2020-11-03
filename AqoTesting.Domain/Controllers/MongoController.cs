using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using MongoDB.Driver;

namespace AqoTesting.Domain.Controllers
{
    public static class MongoController
    {
        public static string? 接続アドレス;
        public static MongoClient? client;
        public static IMongoDatabase mainDatabase;

        public static MongoClient? ConnectToDB(string? ユーザ名, string? パスワード, string ホスト, ushort? ポート, string? データベー, string? オプション)
        {
            接続アドレス = "mongodb://" + (ユーザ名 != null ? $"{ユーザ名}:{パスワード}@" : "") +
                ホスト + (ポート != null ? $":{ポート}" : "") +
                (データベー != null || オプション != null ? "/" : "") +
                (データベー != null ? $"{データベー}" : "") +
                (オプション != null ? $"?options" : "");

            client = new MongoClient(接続アドレス);
            if (client != null && データベー != null) mainDatabase = client.GetDatabase(データベー);

            return client;
        }

        public static IMongoCollection<Room> GetRoomsCollection() => mainDatabase.GetCollection<Room>("rooms");
        public static IMongoCollection<Test> GetTestsCollection() => mainDatabase.GetCollection<Test>("tests");
        public static IMongoCollection<User> GetUsersCollection() => mainDatabase.GetCollection<User>("users");
    }
}
