using AqoTesting.DAL.Controllers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Utils
{
    public class PrepareDB
    {
        public bool CheckMainDatabaseExist()
        {
            using (var cursor = MongoController.client.ListDatabaseNames())
            {
                var databases = cursor.ToList();
                foreach (var database in databases)
                {
                    if (database == "mainAQObase")
                        return true;
                }
            }
            return false;
        }

        public void CreateMainDatabase()
        {
            var mainDatabase = MongoController.client.GetDatabase("mainAQObase");
            MongoController.mainDatabase = mainDatabase;
            mainDatabase.CreateCollection("tests");
            mainDatabase.CreateCollection("users");
            mainDatabase.CreateCollection("rooms");
        }
    }
}
