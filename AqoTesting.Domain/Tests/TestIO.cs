using System;
using AqoTesting.Shared.DTOs.DB.Tests;
using MongoDB.Bson;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Domain.Utils;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Members;
using System.Collections.Generic;

namespace AqoTesting.Domain.Tests
{
    public class TestIO
    {
        public ObjectId[] AddTests(ObjectId userId)
        {
            var tests = new Test[]
            {
                new Test
                {
                    OwnerId = userId,
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
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
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
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                },
                new Test
                {
                    OwnerId = userId,
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
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
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
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                },
                                new Question
                                {
                                    Id = 2,
                                    Text = "Dev test Q 2",
                                    Shuffle = false,
                                    Type = (QuestionTypes) 1,
                                    //OptionsJson = "[{\"text\": \"text1\", \"valid\": true}, {\"text\": \"text2\", \"valid\": false}]"
                                }
                            }
                        }
                    }
                }
            };

            var TestsIds = TestWorker.InsertTests(tests);
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
                    PasswordHash = Sha256.Compute("Pass1"),
                    Name = "Test Dev Name 1",
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Login = "Test Dev Login 2",
                    Email = "DevGavno@gmail.com",
                    PasswordHash = Sha256.Compute("Pass2"),
                    Name = "Test Dev Name 2",
                    RegistrationDate = DateTime.Now
                }
            };

            var UsersIds = UserWorker.InsertUsers(users);
            return UsersIds;
        }

        public ObjectId[] AddMembers()
        {
            var members = new Member[]
            {
                new Member
                {
                    //Token = ObjectId.GenerateNewId().ToString(),
                    Login = "Test Dev Login Member 1",
                    PasswordHash = Sha256.Compute("123"),
                    //Attempts = new Attempt[0],
                    Fields = new Dictionary<string, string>(),
                },
                new Member
                {
                    //Token = ObjectId.GenerateNewId().ToString(),
                    Login = "Test Dev Login Member 2",
                    PasswordHash = Sha256.Compute("123"),
                    //Attempts = new Attempt[0],
                    Fields = new Dictionary<string, string>(),
                }
            };

            var MemberIds = MemberWorker.InsertMembers(members);
            return MemberIds;
        }

        public ObjectId[] AddRooms()
        {
            var users = AddUsers();
            var rooms = new Room[]
            {
                new Room
                {
                    Name = "Test Dev Room",
                    Domain = "Test Dev Domain",
                    //Member = 
                    OwnerId = users[0],
                    //TestIds = AddTests(ObjectId.Parse("5f9211bd5858e9955f588f19")),
                    //OwnerId = ObjectId.Parse("5f9211bd5858e9955f588f19"),
                    IsDataRequired = false,
                    Fields = new RoomField[0],
                    IsActive = true
                }
            };
            var RoomsIds = RoomWorker.InsertRooms(rooms);
            return RoomsIds;
        }
    }
}
