using AqoTesting.DTOs.BDModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Controllers
{
    static class BaseIOController
    {
        public static void AddFullTestObject(FullTest fulltest)
        {
            var sqlStr = "INSERT INTO tests (Id, Title, UserId, CreationDate, ActivationDate, DeactivationDate, Snuffle) VALUES (?Id, ?Title, ?UserId, ?CreationDate, ?ActivationDate, ?DeactivationDate, ?Snuffle)";
            object[,] sqlParams = new object[,] {
                {"Id", fulltest.Test.Id},
                {"Title", fulltest.Test.Title},
                {"UserId", fulltest.Test.UserId},
                {"CreationDate", fulltest.Test.CreationDate},
                {"ActivationDate", fulltest.Test.ActivationDate},
                {"DeactivationDate", fulltest.Test.DeactivationDate},
                {"Snuffle", fulltest.Test.Shuffle},
            };
            BaseController.Insert(sqlStr, sqlParams);
        }
    }
}
