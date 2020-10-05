using AqoTesting.DAL.Utils;
using AqoTesting.DTOs.BDModels;
using AqoTesting.DTOs.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                return ReadTypeHelper.ReadUserInfo(reader);
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
                return ReadTypeHelper.ReadTestInfo(reader);
            }
        }

        public static Test[] GetTestsByUserId(int userId)
        {
            var tests = new List<Test>();
            var query = BaseController.CreateQuery("SELECT * FROM tests where UserId = ?UserId;", new object[,] { { "UserId", userId } });
            using (DbDataReader reader = query.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tests.Add(ReadTypeHelper.ReadTestInfo(reader));
                    }
                }
            }
            return tests.ToArray();
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
                return ReadTypeHelper.ReadSectionInfo(reader);
            }
        }

        public static Section[] GetSectionsByTestId(int testId)
        {
            var sections = new List<Section>();
            var query = BaseController.CreateQuery("SELECT * FROM sections where TestId = ?TestId;", new object[,] { { "TestId", testId } });
            using (DbDataReader reader = query.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sections.Add(ReadTypeHelper.ReadSectionInfo(reader));
                    }
                }
            }
            return sections.ToArray();
        }

        public static SectionWithQuestions[] GetSectionsWithQuestionsByTestId(int testId)
        {
            var sections = new List<SectionWithQuestions>();
            var query = BaseController.CreateQuery("SELECT * FROM sections where TestId = ?TestId;", new object[,] { { "TestId", testId } });
            using (DbDataReader reader = query.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var section = ReadTypeHelper.ReadSectionInfo(reader);
                        sections.Add(new SectionWithQuestions
                        {
                            Id = section.Id,
                            TestId = testId,
                            //Questions = GetQuestionsBySectionId(section.Id) //невозможно открыть 2 ридера
                        });
                    }
                }
            }

            foreach (var section in sections)
            {
                section.Questions = GetQuestionsBySectionId(section.Id);
            }

            return sections.ToArray();
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
                return ReadTypeHelper.ReadQuestionInfo(reader);
            }
        }

        public static Question[] GetQuestionsBySectionId(int sectionId)
        {
            var questions = new List<Question>();
            var query = BaseController.CreateQuery("SELECT * FROM questions where SectionId = ?SectionId;", new object[,] { { "SectionId", sectionId } });
            using (DbDataReader reader = query.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        questions.Add(ReadTypeHelper.ReadQuestionInfo(reader));
                    }
                }
            }
            return questions.ToArray();
        }

        public static FullTestWithFullSections? GetFullTestById(int testId)
        {
            if (BaseController.IsRowWithIdExist("tests", testId))
            {
                return new FullTestWithFullSections
                {
                    Test = GetTestById(testId),
                    Sections = GetSectionsWithQuestionsByTestId(testId)
                };
            } else
            {
                return null;
            }
        }

        #endregion
    }
}
