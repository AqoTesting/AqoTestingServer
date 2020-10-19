using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

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
            //if (client != null && database != null)
            //    mainDatabase = client.GetDatabase(database);
            return client;
        }
    }
}
