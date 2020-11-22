using System;
using System.Collections.Generic;
using System.Linq;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections.Options;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace AqoTesting.WebApi.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;

                #region Users
                cfg.CreateMap<UserAPI_SignUp_DTO, UsersDB_User_DTO>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m => Sha256.Compute(m.Password)))
                    .ForMember(x => x.RegistrationDate,
                        x => x.MapFrom(m => DateTime.UtcNow));

                cfg.CreateMap<UsersDB_User_DTO, AuthUser>()
                    .ForMember(x => x.Role,
                        x => x.MapFrom(m => Role.User));

                cfg.CreateMap<UsersDB_User_DTO, UserAPI_GetProfile_DTO>();
                #endregion

                #region Rooms
                cfg.CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoomsItem_DTO>();

                cfg.CreateMap<UserAPI_PostRoom_DTO, RoomsDB_Room_DTO>();

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_InputField_DTO>();
                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_SelectField_DTO>();

                cfg.CreateMap<RoomsDB_Field_DTO, UserAPI_RoomField_DTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input && m.Data["Placeholder"].IsString ?
                                m.Data["Placeholder"].AsString :
                            null))
                    .ForMember(x => x.Mask,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input && m.Data["Mask"].IsString ?
                                m.Data["Mask"].AsString :
                            null))
                    .ForMember(x => x.Options,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Select && m.Data["Options"].IsBsonArray ?
                                m.Data["Options"].AsBsonArray.Select(item => item.AsString).ToArray() :
                            null));

                cfg.CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<UserAPI_RoomField_DTO[]>(m.Fields)));

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_Field_DTO>()
                    .ForMember(x => x.Data,
                        x => x.ResolveUsing(m =>
                            m.Type == FieldType.Input ?
                                Mapper.Map<RoomsDB_InputField_DTO>(m).ToBsonDocument() :
                            m.Type == FieldType.Select ?
                                Mapper.Map<RoomsDB_SelectField_DTO>(m).ToBsonDocument() :
                            new BsonDocument()));

                cfg.CreateMap<RoomsDB_Field_DTO, MemberAPI_GetRoomField_DTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input && m.Data["Placeholder"].IsString ?
                                m.Data["Placeholder"].AsString :
                            null))
                    .ForMember(x => x.Mask,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input && m.Data["Mask"].IsString ?
                                m.Data["Mask"].AsString :
                            null))
                    .ForMember(x => x.Options,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Select && m.Data["Options"].IsBsonArray ?
                                m.Data["Options"].AsBsonArray.Select(item => item.AsString).ToArray() :
                            null));

                cfg.CreateMap<RoomsDB_Room_DTO, MemberAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<MemberAPI_GetRoomField_DTO[]>(m.Fields)
                        ));
                #endregion

                #region Tests
                cfg.CreateMap<TestsDB_ChoiceOption, UserAPI_CommonOption_DTO>();
                cfg.CreateMap<TestsDB_Question_DTO, UserAPI_GetQuestion_DTO>()
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice)
                            {
                                return Mapper.Map<UserAPI_CommonOption_DTO[]>( BsonSerializer.Deserialize<TestsDB_ChoiceOption[]>(m.Options) );
                            }
                            else if(m.Type == QuestionTypes.Matching)
                            {
                                return Mapper.Map<UserAPI_MatchingOption_DTO>( BsonSerializer.Deserialize<TestsDB_MatchingOptionsData>(m.Options));
                            }
                            else
                            {
                                return new UserAPI_CommonOption_DTO[];
                            }
                        }));
                cfg.CreateMap<TestsDB_Section_DTO, UserAPI_GetSection_DTO>()
                    .ForMember(x => x.Questions,
                        x => x.MapFrom(m => Mapper.Map<Dictionary<int, UserAPI_GetQuestion_DTO>>(m.Questions)));
                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTest_DTO>()
                    .ForMember(x => x.Sections,
                        x => x.MapFrom(m => Mapper.Map<Dictionary<int, UserAPI_GetSection_DTO>>(m.Sections)));

                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTestsItem_DTO>();

                cfg.CreateMap<UserAPI_ChoiceOption_DTO, TestsDB_ChoiceOption>();
                cfg.CreateMap<UserAPI_MatchingOption_DTO, TestsDB_PositionalOption>();

                cfg.CreateMap<UserAPI_PostQuestion_DTO, TestsDB_Question_DTO>()
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                            m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                                new TestsDB_OptionsData {
                                    Data = Mapper.Map<TestsDB_ChoiceOption[]>(m.Options)
                                }.ToBsonDocument() :

                            m.Type == QuestionTypes.Matching ?
                                new TestsDB_MatchingOptionsData {
                                    Left = m.Options.Select(option => new TestsDB_PositionalOption {
                                        Text = option.LeftText,
                                        ImageUrl = option.LeftImageUrl
                                    }).ToArray(),
                                    Right = m.Options.Select(option => new TestsDB_PositionalOption {
                                        Text = option.RightText,
                                        ImageUrl = option.RightImageUrl
                                    }).ToArray()
                                }.ToBsonDocument() :

                            m.Type == QuestionTypes.Sequence ?
                                new TestsDB_OptionsData {
                                    Data = Mapper.Map<TestsDB_PositionalOption[]>(m.Options)
                                }.ToBsonDocument() :
                            
                            new BsonDocument()));

                cfg.CreateMap<UserAPI_PostSection_DTO, TestsDB_Section_DTO>()
                    .ForMember(x => x.Questions,
                        x => x.MapFrom(m => Mapper.Map<Dictionary<int, TestsDB_Question_DTO>>(m.Questions)));

                cfg.CreateMap<UserAPI_PostTest_DTO, TestsDB_Test_DTO>();
                #endregion

                #region Members
                cfg.CreateMap<MembersDB_Member_DTO, UserAPI_GetMembersItem_DTO>();

                cfg.CreateMap<UserAPI_PostMember_DTO, MembersDB_Member_DTO>();

                cfg.CreateMap<MemberAPI_SignUp_DTO, MembersDB_Member_DTO>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m =>
                            Sha256.Compute(m.Password)))
                    .ForMember(x => x.RoomId,
                        x => x.MapFrom(m =>
                            ObjectId.Parse(m.RoomId)));
                #endregion
            });
        }
    }
}
