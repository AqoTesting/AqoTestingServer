using AqoTesting.DAL.Utils;
using AqoTesting.DTOs.BDModels;
using AqoTesting.DTOs.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace AqoTesting.DAL.Controllers
{
    public static class BaseIOController
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

        #region readFromDB

        public static User GetUserById(int userId)
        {
            var query = BaseController.GetRowById("users", userId);
            if (query == null)
                return null;
            else
            {
                using DbDataReader reader = query.ExecuteReader();
                reader.Read();
                User user = new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Login = reader.GetString(reader.GetOrdinal("Login")),
                    Name = reader.GetStringOrNull(reader.GetOrdinal("Name")),
                    RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
                };
                //Console.WriteLine(user.Id);
                //Console.WriteLine(user.Login);
                //Console.WriteLine(user.Name);
                //Console.WriteLine(user.RegistrationDate);
                return user;
            }
        }

        public static Test GetTestById(int testId)
        {
            var query = BaseController.GetRowById("tests", testId);
            if (query == null)
                return null;
            else
            {
                using DbDataReader reader = query.ExecuteReader();
                reader.Read();
                Test test = new Test
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                    ActivationDate = reader.GetDateTimeOrNull(reader.GetOrdinal("ActivationDate")),
                    DeactivationDate = reader.GetDateTimeOrNull(reader.GetOrdinal("DeactivationDate")),
                    Shuffle = reader.GetBoolean(reader.GetOrdinal("Title")),
                };
                return test;
            }
        }

        public static Section GetSectionById(int sectionId)
        {
            var query = BaseController.GetRowById("sections", sectionId);
            if (query == null)
                return null;
            else
            {
                using DbDataReader reader = query.ExecuteReader();
                reader.Read();
                Section section = new Section
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    TestId = reader.GetInt32(reader.GetOrdinal("TestId")),
                };
                return section;
            }
        }

        public static Question GetQuestionById(int questionId)
        {
            var query = BaseController.GetRowById("questions", questionId);
            if (query == null)
                return null;
            else
            {
                using DbDataReader reader = query.ExecuteReader();
                reader.Read();
                Question question = new Question
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                    Type = (QuestionTypeEnum) reader.GetInt32(reader.GetOrdinal("Type")),
                    Text = reader.GetStringOrNull(reader.GetOrdinal("Text")),
                    OptionsJson = reader.GetString(reader.GetOrdinal("OptionsJson")),
                    Shuffle = reader.GetBoolean(reader.GetOrdinal("Shuffle")),
                };
                return question;
            }
        }

        #endregion
    }
}
