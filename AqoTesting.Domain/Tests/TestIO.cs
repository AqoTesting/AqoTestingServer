using AqoTesting.Domain.Controllers;
using AqoTesting.Core.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using AqoTesting.Core.DTOs.BD;

namespace AqoTesting.Domain.Tests
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
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        },
                        new Section
                        {
                            Id = 2,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
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
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        },
                        new Section
                        {
                            Id = 2,
                            Questions = new Question[]
                            {
                                new Question
                                {
                                    Id = 1,
                                    Text = "Dev test Q 1",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (Core.Enums.QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                }
            };

            MongoController.mainDatabase.GetCollection<Test>("tests").InsertMany(tests);
        }

        public Test GetTestById(int testId)
        {
            var collection = MongoController.mainDatabase.GetCollection<Test>("tests");
            var entity = collection.Find(Builders<Test>.Filter.Eq("id", testId));
            var test = entity.SingleOrDefault();
            return test;
        }

        public void AddUsers()
        {
            var users = new User[]
            {
                new User
                {
                    Id = 1,
                    Login = "Test Dev Login 1",
                    Name = "Test Dev Name 1",
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Login = "Test Dev Login 2",
                    Name = "Test Dev Name 2",
                    RegistrationDate = DateTime.Now
                }
            };

            MongoController.mainDatabase.GetCollection<User>("users").InsertMany(users);
        }

        public User GetUserById(int userId)
        {
            var collection = MongoController.mainDatabase.GetCollection<User>("users");
            var entity = collection.Find(Builders<User>.Filter.Eq("id", userId));
            var user = entity.SingleOrDefault();
            return user;
        }
    }
}
