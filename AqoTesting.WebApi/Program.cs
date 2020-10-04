using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.DAL.Dev_Tests;
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
            new AqoTesting.DAL.DBCreator().Init();
            CreateHostBuilder(args).Build().Run();
            new Dev_CreateTest().CreateTest();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
