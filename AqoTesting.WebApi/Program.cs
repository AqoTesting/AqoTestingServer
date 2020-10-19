using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Domain.Controllers;
using AqoTesting.Domain.Tests;
using AqoTesting.Domain.Utils;
using AqoTesting.Core.DTOs.BD;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AqoTestingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*MongoController.ConnectToDB(null, null, "localhost", null, null, null);
            MongoController.client.DropDatabase("mainAQObase");

            PrepareDB prepareDB = new PrepareDB();
            var isMainDatabaseExist = prepareDB.CheckMainDatabaseExist();
            if (!isMainDatabaseExist)
            {
                Console.WriteLine("Creating");
                prepareDB.CreateMainDatabase();
                var testIO = new TestIO();
                testIO.AddTests();
                Console.WriteLine(testIO.GetUserById(1));
            }*/

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
