using AqoTesting.DTOs.BDModels;
using AqoTesting.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AqoTesting.DAL.Utils
{
    public static class ReadTypeHelper
    {
        public static User ReadUserInfo(DbDataReader reader)
        {
            User user = new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Login = reader.GetString(reader.GetOrdinal("Login")),
                Name = reader.GetStringOrNull(reader.GetOrdinal("Name")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
            };
            return user;
        }

        public static Test ReadTestInfo(DbDataReader reader)
        {
            Test test = new Test
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                ActivationDate = reader.GetDateTimeOrNull(reader.GetOrdinal("ActivationDate")),
                DeactivationDate = reader.GetDateTimeOrNull(reader.GetOrdinal("DeactivationDate")),
                Shuffle = reader.GetBoolean(reader.GetOrdinal("Shuffle")),
            };
            return test;
        }

        public static Section ReadSectionInfo(DbDataReader reader)
        {
            Section section = new Section
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                TestId = reader.GetInt32(reader.GetOrdinal("TestId")),
            };
            return section;
        }

        public static Question ReadQuestionInfo(DbDataReader reader)
        {
            Question question = new Question
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                Type = (QuestionTypeEnum)reader.GetInt32(reader.GetOrdinal("Type")),
                Text = reader.GetStringOrNull(reader.GetOrdinal("Text")),
                OptionsJson = reader.GetString(reader.GetOrdinal("OptionsJson")),
                Shuffle = reader.GetBoolean(reader.GetOrdinal("Shuffle")),
            };
            return question;
        }
    }
}
