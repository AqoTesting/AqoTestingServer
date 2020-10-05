using AqoTesting.DAL.Controllers;
using AqoTesting.DTOs.BDModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Dev_Tests
{
    public class Dev_CreateTest
    {
        public void CreateTest()
        {
            if (!BaseController.IsRowWithIdExist("users", 11))
            {
                User user = new User
                {
                    Id = 11,
                    Login = "Test Dev User",
                    Name = "DevFirstName DevLastName",
                    RegistrationDate = DateTime.Now
                };

                Test test = new Test
                {
                    Id = 10,
                    Title = "Test Dev Test",
                    UserId = 11,
                    CreationDate = DateTime.Now,
                    ActivationDate = null,
                    DeactivationDate = null,
                    Shuffle = false
                };

                Section section = new Section
                {
                    Id = 9,
                    TestId = 10
                };

                Question question1 = new Question
                {
                    Id = 8,
                    SectionId = 9,
                    Type = (DTOs.Enums.QuestionTypeEnum)1,
                    Text = "Dev Question1 Text",
                    Shuffle = true,
                    OptionsJson = "[{ \"Text\": \"Valid1\", \"Correct\":true },{ \"Text\": \"Valid2\", \"Correct\":true },{ \"Text\": \"Invalid\", \"Correct\":false }]"
                };

                Question question2 = new Question
                {
                    Id = 7,
                    SectionId = 9,
                    Type = (DTOs.Enums.QuestionTypeEnum)2,
                    Text = "Dev Question2 Text",
                    Shuffle = true,
                    OptionsJson = "[{ \"Text\": \"Valid\", \"Correct\":true },{ \"Text\": \"Invalid1\", \"Correct\":false },{ \"Text\": \"Invalid2\", \"Correct\":false }]"
                };

                FullTest fullTest = new FullTest
                {
                    Test = test,
                    Sections = new Section[] { section },
                    Questions = new Question[] { question1, question2 }
                };

                BaseIOController.InsertFullTestObject(fullTest);
                BaseIOController.InsertUserObject(user);
            }
        }
    }
}
