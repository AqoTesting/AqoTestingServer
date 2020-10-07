using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.DAL.Controllers;
using AqoTesting.DAL.Tests;
using AqoTesting.DAL.Utils;
using AqoTesting.DTOs.BDModels;
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
            MongoController.ConnectToDB(null, null, "localhost", null, null, null);
            MongoController.client.DropDatabase("mainAQObase");

            PrepareDB prepareDB = new PrepareDB();
            var isMainDatabaseExist = prepareDB.CheckMainDatabaseExist();
            if (!isMainDatabaseExist)
            {
                Console.WriteLine("Creating");
                prepareDB.CreateMainDatabase();
                var testIO = new TestIO();
                testIO.AddTests();
            }

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
