using AqoTesting.Domain.Controllers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using AqoTesting.Shared.DTOs.BD.Tests;
using MongoDB.Bson;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.BD.Users;

namespace AqoTesting.Domain.Tests
{
    public class TestIO
    {
        public ObjectId[] AddTests()
        {
            var tests = new Test[]
            {
                new Test
                {
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
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypeEnum) 1,
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
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                },
                new Test
                {
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
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypeEnum) 1,
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
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypeEnum) 1,
                                    OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                }
            };
            MongoController.mainDatabase.GetCollection<Test>("tests").InsertMany(tests);
            var TestsIds = new List<ObjectId>();
            foreach (var test in tests)
            {
                Console.WriteLine("TestId: " + test.Id);
                TestsIds.Add(test.Id);
            }

            return TestsIds.ToArray();
        }

        public ObjectId[] AddUsers()
        {
            var users = new User[]
            {
                new User
                {
                    Login = "Test Dev Login 1",
                    Name = "Test Dev Name 1",
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Login = "Test Dev Login 2",
                    Name = "Test Dev Name 2",
                    RegistrationDate = DateTime.Now
                }
            };

            MongoController.mainDatabase.GetCollection<User>("users").InsertMany(users);
            var UsersIds = new List<ObjectId>();
            foreach (var user in users)
            {
                Console.WriteLine("UserId: " + user.Id);
                UsersIds.Add(user.Id);
            }

            return UsersIds.ToArray();
        }
    }
}
