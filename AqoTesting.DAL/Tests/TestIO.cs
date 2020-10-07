using AqoTesting.DAL.Controllers;
using AqoTesting.DTOs.BDModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Tests
{
    public class TestIO
    {
        public void AddTests()
        {
            var tests = new Test[]
            {
                new Test
                {
                    Id = 1,
                    UserId = 1,
                    CreationDate = DateTime.Now,
                    ActivationDate = null,
                    DeactivationDate = null,
                    Shuffle = true,
                    Title = "Dev 1 Test",
                    Sections = new Section[]
                    {
                        new Section
                        {
                            Id = 1,
                            TestId = 1,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    SectionId = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    SectionId = 1,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        },
                        new Section
                        {
                            Id = 2,
                            TestId = 1,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    SectionId = 2,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    SectionId = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                },
                new Test
                {
                    Id = 2,
                    UserId = 1,
                    CreationDate = DateTime.Now,
                    ActivationDate = null,
                    DeactivationDate = null,
                    Shuffle = true,
                    Title = "Dev 2 Test",
                    Sections = new Section[]
                    {
                        new Section
                        {
                            Id = 1,
                            TestId = 2,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    SectionId = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    SectionId = 1,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        },
                        new Section
                        {
                            Id = 2,
                            TestId = 2,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    SectionId = 2,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    SectionId = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (DTOs.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                }
            };

            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            collection.InsertMany(tests);
        }
    }
}
