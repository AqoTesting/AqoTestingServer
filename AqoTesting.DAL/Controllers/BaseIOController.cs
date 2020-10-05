using AqoTesting.DTOs.BDModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Controllers
{
    static class BaseIOController
    {
        public static void InsertFullTestObject(FullTest fulltest)
        {
            //Test inserting
            var sqlStr = "INSERT INTO tests (Id, Title, UserId, CreationDate, ActivationDate, DeactivationDate, Shuffle) VALUES (?Id, ?Title, ?UserId, ?CreationDate, ?ActivationDate, ?DeactivationDate, ?Shuffle)";
            object[,] sqlParams = new object[,] {
                {"Id", fulltest.Test.Id},
                {"Title", fulltest.Test.Title},
                {"UserId", fulltest.Test.UserId},
                {"CreationDate", fulltest.Test.CreationDate},
                {"ActivationDate", fulltest.Test.ActivationDate},
                {"DeactivationDate", fulltest.Test.DeactivationDate},
                {"Shuffle", fulltest.Test.Shuffle},
            };
            BaseController.ExecuteQuery(sqlStr, sqlParams);

            //Section inserting
            sqlStr = "INSERT INTO sections (Id, TestId) VALUES (?Id, ?TestId)";
            foreach (var section in fulltest.Sections)
            {
                sqlParams = new object[,] {
                    {"Id", section.Id},
                    {"TestId", section.TestId}
                };
                BaseController.ExecuteQuery(sqlStr, sqlParams);
            }

            //Question inserting
            sqlStr = "INSERT INTO questions (Id, SectionId, Type, Text, Shuffle, OptionsJson) VALUES (?Id, ?SectionId, ?Type, ?Text, ?Shuffle, ?OptionsJson)";
            foreach (var question in fulltest.Questions)
            {
                sqlParams = new object[,] {
                    {"Id", question.Id},
                    {"SectionId", question.SectionId},
                    {"Type", (byte) question.Type},
                    {"Text", question.Text},
                    {"Shuffle", question.Shuffle},
                    {"OptionsJson", question.OptionsJson}
                };
                BaseController.ExecuteQuery(sqlStr, sqlParams);
            }
        }

        public static void InsertUserObject(User user)
        {
            var sqlStr = "INSERT INTO users (Id, Login, Name, RegistrationDate) VALUES (?Id, ?Login, ?Name, ?RegistrationDate)";
            var sqlParams = new object[,] {
                {"Id", user.Id},
                {"Login", user.Login},
                {"Name", user.Name},
                {"RegistrationDate", user.RegistrationDate}
            };
            BaseController.ExecuteQuery(sqlStr, sqlParams);
        }
    }
}
