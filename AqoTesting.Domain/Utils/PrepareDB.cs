using AqoTesting.Domain.Controllers;
using MongoDB.Driver;

namespace AqoTesting.Domain.Utils
{
    public class PrepareDB
    {
        public bool CheckMainDatabaseExist()
        {
            using(var cursor = MongoController.client.ListDatabaseNames())
            {
                var databases = cursor.ToList();
                foreach(var database in databases)
                {
                    if(database == "mainAQObase")
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
            mainDatabase.CreateCollection("members");
        }
    }
}
