using System;
using AqoTesting.Domain.Controllers;
using AqoTesting.Domain.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AqoTestingServer
{
    class Selector
    {
        public string[] Options { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            MongoController.ConnectToDB(null, null, "144.76.197.194", 23565, "mainAQObase", null);
            //MongoController.client.DropDatabase("mainAQObase");

            PrepareDB prepareDB = new PrepareDB();
            if(!prepareDB.CheckMainDatabaseExist())
            {
                prepareDB.CreateMainDatabase();
            }
            Console.WriteLine("OK");
            //var testIO = new TestIO();
            //UserWorker.DeleteUserByLogin("Test Dev Login 1");
            //UserWorker.DeleteUserByLogin("Test Dev Login 2");
            //var membersIds = testIO.AddMembers();
            //var roomsIds = testIO.AddRooms();
            //var mainRoom = RoomWorker.UserAPI_GetRoomById(roomsIds[0]);
            //
            // ���������� ������� �� id �� ���� (�������� ���������� mainRoom.AddMember)
            //mainRoom.AddMember(membersIds[0]);
            //
            // ���������� ������� ����� ������������� ���������� ������ (�������� ��� ��� � ����) ������������ (�������� ������� member.RoomId � ������������ MemberWorker.SetMemberRoomId �� ���������!)
            //mainRoom.AddMember(new member...);

            // ������ ������� �� ����� � ����
            //member.Delete();
            //MemberWorker.DeleteMember(ObjectId.Parse("5fa0a46a5c7cfd09d8b19286"));
            //MemberWorker.DeleteMember(member);

            // ������� �������� ����
            //var testIO = new TestIO();
            //UserWorker.DeleteUserByLogin("Test Dev Login 1");
            //UserWorker.DeleteUserByLogin("Test Dev Login 2");
            //var membersIds = testIO.AddMembers();
            //var roomsIds = testIO.AddRooms();
            //var mainRoom = RoomWorker.UserAPI_GetRoomById(roomsIds[0]);
            //mainRoom.AddMember(membersIds[0]);
            //mainRoom.AddMember(membersIds[1]);

            // ���� �������
            //var mainRoom = RoomWorker.UserAPI_GetRoomByDomain("Test Dev Domain");
            //var mainField = new RoomField
            //{
            //    Name = "Main Field",
            //    Type = FieldType.Select,
            //    IsRequired = true,
            //    Data = new Selector
            //    {
            //        Options = new string[]
            //        {
            //            "�����1",
            //            "�����2",
            //            "�����3",
            //            "�����4",
            //            "�����5",
            //            "�����6",
            //        }
            //    }.ToBsonDocument()
            //};
            //mainRoom.AddField(mainField);
            //Console.WriteLine(mainRoom.GetFields().Length);
            //var field = mainRoom.GetFields()[0];
            //Console.WriteLine(field.Name);
            //Console.WriteLine(field.Type);
            //Console.WriteLine(field.Data);
            //switch (field.Type)
            //{
            //    case FieldType.Input:
            //        /// ���-��
            //        break;
            //    case FieldType.Select:
            //        var selector = BsonSerializer.Deserialize<Selector>(field.Data);
            //        foreach (var option in selector.Options)
            //            Console.WriteLine(option);
            //        break;
            //}
            //mainRoom.RemoveField(mainRoom.GetFields()[0].Name);

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
