using AqoTesting.Domain.Controllers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using AqoTesting.Shared.DTOs.BD.Tests;
using MongoDB.Bson;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.BD.Users;
using AqoTesting.Shared.DTOs.BD.Rooms;
using AqoTesting.Shared.DTOs.BD;

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

            var TestsIds = MongoIOController.InsertTests(tests);
            return TestsIds;
        }

        public ObjectId[] AddUsers()
        {
            var users = new User[]
            {
                new User
                {
                    Login = "Test Dev Login 1",
                    Email = "DevGavno@gmail.com",
                    Name = "Test Dev Name 1",
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Login = "Test Dev Login 2",
                    Email = "DevGavno@gmail.com",
                    Name = "Test Dev Name 2",
                    RegistrationDate = DateTime.Now
                }
            };

            var UsersIds = MongoIOController.InsertUsers(users);
            return UsersIds;
        }

        public ObjectId[] AddRooms()
        {
            var rooms = new Room[]
            {
                new Room
                {
                    Name = "Test Dev Room",
                    Domain = "Test Dev Domain",
                    Members = new Member[]
                    {
                        new Member
                        {
                            Token = ObjectId.GenerateNewId().ToString(),
                            Login = "Test Dev Login Member 1",
                            Password = "123",
                            Attempts = new Attempt[0],
                            UserData = new object(),
                        },
                        new Member
                        {
                            Token = ObjectId.GenerateNewId().ToString(),
                            Login = "Test Dev Login Member 2",
                            Password = "123",
                            Attempts = new Attempt[0],
                            UserData = new object(),
                        }
                    },
                    TestIds = AddTests(),
                    OwnerId = AddUsers()[0],
                    IsDataRequire = false,
                    RequestedFields = new RequestedField[0],
                    IsActive = true
                }
            };
            var RoomsIds = MongoIOController.InsertRooms(rooms);
            return RoomsIds;
        }
    }
}
