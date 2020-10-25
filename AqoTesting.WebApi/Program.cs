using System;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Domain.Tests;
using AqoTesting.Domain.Utils;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace AqoTestingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MongoController.ConnectToDB(null, null, "144.76.197.194", 23565, "mainAQObase", null);
            //MongoController.client.DropDatabase("mainAQObase");

            PrepareDB prepareDB = new PrepareDB();
            var isMainDatabaseExist = prepareDB.CheckMainDatabaseExist();
            if (isMainDatabaseExist)
            {

                Console.WriteLine("OK");
                /*prepareDB.CreateMainDatabase();
                var testIO = new TestIO();
                //var testsIds = testIO.AddTests();
                //var usersIds = testIO.AddUsers();
                var roomsIds = testIO.AddRooms();
                foreach (var id in roomsIds)
                    Console.WriteLine(id.ToString());
                //Console.WriteLine(MongoIOController.GetUserById((ObjectId) DBUtils.ParseObjectId("42d3076b46319251f02bc896")) == null);
                //Console.WriteLine("Find user with id " + usersIds[0].ToString());
                //var user = MongoIOController.GetUserById(usersIds[0]);
                //Console.WriteLine(user.Name);
                using SHA512 sha512Hash = SHA512.Create();
                var user = MongoIOController.GetUserByData("Test Dev Login 2", sha512Hash.ComputeHash(Encoding.UTF8.GetBytes("Pass2")));
                if (user != null)
                {
                    Console.WriteLine(user.Name);
                    //MongoIOController.DeleteUserById(user.Id);
                }*/
            }
            else
            {
                prepareDB.CreateMainDatabase();
            }

            //var user = UserWorker.GetUserById(ObjectId.Parse("5f92b5ba5f039b5caf621df7"));
            //user.SetName("Test Name");

            //var test = TestWorker.GetTestById(ObjectId.Parse("5f92b5b95f039b5caf621df6"));
            //test.SetTitle("test title");
            //test.SetUserId(ObjectId.Parse("5f92b5ba5f039b5caf621df7"));
            //test.SetActivationDate(DateTime.Now);
            //test.SetDeactivationDate(DateTime.Now.AddDays(1));

            //var room = RoomWorker.GetRoomById(ObjectId.Parse("5f92b5ba5f039b5caf621df9"));
            /*
            room.AddMember(new Member
            {
                Token = ObjectId.GenerateNewId().ToString(),
                Login = "test new",
                Password = "123",
                Attempts = new Attempt[0],
                UserData = new object(),
            });
            */
            //room.RemoveMemberByToken("5f92b5b95f039b5caf621df3");

            //var testIO = new TestIO();
            //var usersIds = testIO.AddUsers();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
