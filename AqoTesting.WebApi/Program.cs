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
            if (!prepareDB.CheckMainDatabaseExist())
            {
                prepareDB.CreateMainDatabase();
            }
            Console.WriteLine("OK");
            //var testIO = new TestIO();
            //UserWorker.DeleteUserByLogin("Test Dev Login 1");
            //UserWorker.DeleteUserByLogin("Test Dev Login 2");
            //var membersIds = testIO.AddMembers();
            //var roomsIds = testIO.AddRooms();
            //var mainRoom = RoomWorker.GetRoomById(roomsIds[0]);
            //
            // добавление мембера по id из базы (прив€зку произведет mainRoom.AddMember)
            //mainRoom.AddMember(membersIds[0]);
            //
            // добавление мембера через использование экземпл€ра Ќќ¬ќ√ќ (которого еще нет в базе) пользовател€ (задавать вручную member.RoomId и использовать MemberWorker.SetMemberRoomId не требуетс€!)
            //mainRoom.AddMember(new member...);

            // ”далит мембера из теста и базы
            //member.Delete();
            //MemberWorker.DeleteMember(ObjectId.Parse("5fa0a46a5c7cfd09d8b19286"));
            //MemberWorker.DeleteMember(member);

            //var testIO = new TestIO();
            //UserWorker.DeleteUserByLogin("Test Dev Login 1");
            //UserWorker.DeleteUserByLogin("Test Dev Login 2");
            //var membersIds = testIO.AddMembers();
            //var roomsIds = testIO.AddRooms();
            //var mainRoom = RoomWorker.GetRoomById(roomsIds[0]);
            //mainRoom.AddMember(membersIds[0]);
            //mainRoom.AddMember(membersIds[1]);

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
