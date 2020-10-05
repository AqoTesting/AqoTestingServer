using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.DAL.Controllers;
using AqoTesting.DAL.Dev_Tests;
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
            new AqoTesting.DAL.Utils.DBCreator().Init();
            new Dev_CreateTest().CreateTest();
            BaseIOController.GetUserById(11);
            var fullTest = BaseIOController.GetFullTestById(10).GetValueOrDefault();
            Console.WriteLine(fullTest.Sections.Length);
            foreach (var section in fullTest.Sections)
            {
                Console.WriteLine(section.Questions.Length);
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
