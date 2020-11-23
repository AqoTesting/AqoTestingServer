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
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input ?
                                Mapper.Map<RoomsDB_InputField_DTO>(m).ToBsonDocument(null, null, default) :
                            m.Type == FieldType.Select ?
                                Mapper.Map<RoomsDB_SelectField_DTO>(m).ToBsonDocument(null, null, default) :
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
                #region TestsDB_Test_DTO -> UserAPI_GetTest_DTO
                cfg.CreateMap<TestsDB_ChoiceOption, UserAPI_CommonOption_DTO>();
                cfg.CreateMap<TestsDB_MatchingOptionsData, UserAPI_CommonOption_DTO[]>()
                    .ConstructUsing(x =>
                    {
                        var commonOptionsDTOs = new UserAPI_CommonOption_DTO[x.Left.Length];
                        for(var i = 0; i < x.Left.Length; i++)
                            commonOptionsDTOs[i] = new UserAPI_CommonOption_DTO {
                                LeftText = x.Left[i].Text,
                                LeftImageUrl = x.Left[i].Text,
                                RightText = x.Right[i].Text,
                                RightImageUrl = x.Right[i].ImageUrl
                            };

                        return commonOptionsDTOs;
                    });
                cfg.CreateMap<TestsDB_PositionalOption, UserAPI_CommonOption_DTO>();
                cfg.CreateMap<TestsDB_Question_DTO, UserAPI_GetQuestion_DTO>()
                    .ForMember(x => x.Options,
                        x => x.MapFrom(m =>
                            m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                                Mapper.Map<UserAPI_CommonOption_DTO[]>((TestsDB_ChoiceOption[]) BsonSerializer.Deserialize<TestsDB_OptionsData>(m.Options, null).Data) :

                            m.Type == QuestionTypes.Matching ?
                                Mapper.Map<UserAPI_CommonOption_DTO[]>(BsonSerializer.Deserialize<TestsDB_MatchingOptionsData>(m.Options, null)) :
                            
                            m.Type == QuestionTypes.Sequence ?
                                Mapper.Map<UserAPI_CommonOption_DTO[]>((TestsDB_PositionalOption[]) BsonSerializer.Deserialize<TestsDB_OptionsData>(m.Options, null).Data) :

                            new UserAPI_CommonOption_DTO[0]));
                cfg.CreateMap<KeyValuePair<string, TestsDB_Question_DTO>, KeyValuePair<string, UserAPI_GetQuestion_DTO>>()
                    .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetQuestion_DTO>(
                        x.Key,
                        Mapper.Map<UserAPI_GetQuestion_DTO>(x.Value)));
                cfg.CreateMap<TestsDB_Section_DTO, UserAPI_GetSection_DTO>()
                    .ForMember(x => x.Questions,
                        x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetQuestion_DTO>>(m.Questions)));
                cfg.CreateMap<KeyValuePair<string, TestsDB_Section_DTO>, KeyValuePair<string, UserAPI_GetSection_DTO>>()
                    .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetSection_DTO>(
                        x.Key,
                        Mapper.Map<UserAPI_GetSection_DTO>(x.Value)));
                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTest_DTO>()
                    .ForMember(x => x.Sections,
                        x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetSection_DTO>>(m.Sections)));
                #endregion

                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTestsItem_DTO>();

                #region UserAPI_PostSection_DTO -> TestsDB_Section_DTO
                cfg.CreateMap<UserAPI_CommonOption_DTO, TestsDB_ChoiceOption>();
                cfg.CreateMap<UserAPI_CommonOption_DTO[], TestsDB_MatchingOptionsData>()
                    .ForMember(x => x.Left,
                        x => x.MapFrom(m =>
                            m.Select(option => new TestsDB_PositionalOption {
                                Text = option.LeftText,
                                ImageUrl = option.LeftImageUrl
                            })))
                    .ForMember(x => x.Right,
                        x => x.MapFrom(m =>
                            m.Select(option => new TestsDB_PositionalOption {
                                Text = option.RightText,
                                ImageUrl = option.RightImageUrl
                            })));
                cfg.CreateMap<UserAPI_CommonOption_DTO, TestsDB_PositionalOption>();
                cfg.CreateMap<UserAPI_PostQuestion_DTO, TestsDB_Question_DTO>()
                    .ForMember(x => x.Options,
                        x => x.MapFrom(m =>
                            m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                                new TestsDB_OptionsData { 
                                    Data = Mapper.Map<TestsDB_ChoiceOption[]>(m.Options)
                                }.ToBsonDocument(null, null, default) :

                            m.Type == QuestionTypes.Matching ?
                                Mapper.Map<TestsDB_MatchingOptionsData>(m.Options).ToBsonDocument(null, null, default) :

                            m.Type == QuestionTypes.Sequence ?
                                new TestsDB_OptionsData { 
                                    Data = Mapper.Map<TestsDB_PositionalOption[]>(m.Options)
                                }.ToBsonDocument(null, null, default) :

                            new BsonDocument()));
                cfg.CreateMap<KeyValuePair<string, UserAPI_PostQuestion_DTO>, KeyValuePair<string, TestsDB_Question_DTO>>()
                    .ConstructUsing(x => new KeyValuePair<string, TestsDB_Question_DTO>(
                        x.Key,
                        Mapper.Map<TestsDB_Question_DTO>(x.Value)));
                cfg.CreateMap<UserAPI_PostSection_DTO, TestsDB_Section_DTO>()
                    .ForMember(x => x.Questions,
                        x => x.MapFrom(m =>
                            Mapper.Map<Dictionary<string, TestsDB_Question_DTO>>(m.Questions)));
                #endregion

                cfg.CreateMap<UserAPI_PostTest_DTO, TestsDB_Test_DTO>()
                    .ForMember(x => x.Sections,
                        x => x.MapFrom(m =>
                            new Dictionary<string, UserAPI_PostSections_DTO>()));
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
