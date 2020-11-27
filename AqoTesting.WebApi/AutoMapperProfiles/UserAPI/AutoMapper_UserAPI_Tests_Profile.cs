﻿using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Tests.Options;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_Tests_Profile : Profile
    {
        public AutoMapper_UserAPI_Tests_Profile()
        {
            #region DB -> API
            #region TestsDB_Test_DTO -> UserAPI_GetTest_DTO
            CreateMap<TestsDB_ChoiceOption, UserAPI_TestCommonOption_DTO>();
            CreateMap<TestsDB_MatchingOptionsData, UserAPI_TestCommonOption_DTO[]>()
                .ConstructUsing(x => {
                    var commonOptionDTOs = new UserAPI_TestCommonOption_DTO[x.Left.Length];
                    for (var i = 0; i < x.Left.Length; i++)
                        commonOptionDTOs[i] = new UserAPI_TestCommonOption_DTO {
                            LeftText = x.Left[i].Text,
                            LeftImageUrl = x.Left[i].ImageUrl,
                            RightText = x.Right[i].Text,
                            RightImageUrl = x.Right[i].ImageUrl };

                    return commonOptionDTOs;
                });
            CreateMap<TestsDB_PositionalOption, UserAPI_TestCommonOption_DTO>();
            CreateMap<TestsDB_Question_DTO, UserAPI_GetTestQuestion_DTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<UserAPI_TestCommonOption_DTO[]>((TestsDB_ChoiceOption[])BsonSerializer.Deserialize<TestsDB_OptionsData>(m.Options, null).Data) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<UserAPI_TestCommonOption_DTO[]>(BsonSerializer.Deserialize<TestsDB_MatchingOptionsData>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<UserAPI_TestCommonOption_DTO[]>((TestsDB_PositionalOption[]) BsonSerializer.Deserialize<TestsDB_OptionsData>(m.Options, null).Data) :

                        new UserAPI_TestCommonOption_DTO[0]));
            CreateMap<KeyValuePair<string, TestsDB_Question_DTO>, KeyValuePair<string, UserAPI_GetTestQuestion_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetTestQuestion_DTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetTestQuestion_DTO>(x.Value)));
            CreateMap<TestsDB_Section_DTO, UserAPI_GetTestSection_DTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetTestQuestion_DTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, TestsDB_Section_DTO>, KeyValuePair<string, UserAPI_GetTestSection_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetTestSection_DTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetTestSection_DTO>(x.Value)));
            CreateMap<TestsDB_Test_DTO, UserAPI_GetTest_DTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetTestSection_DTO>>(m.Sections)));
            #endregion

            CreateMap<TestsDB_Test_DTO, UserAPI_GetTestsItem_DTO>();
            #endregion

            #region API -> DB
            #region UserAPI_PostSection_DTO -> TestsDB_Section_DTO
            CreateMap<UserAPI_TestCommonOption_DTO, TestsDB_ChoiceOption>();
            CreateMap<UserAPI_TestCommonOption_DTO[], TestsDB_MatchingOptionsData>()
                .ForMember(x => x.Left,
                    x => x.MapFrom(m =>
                        m.Select(option => new TestsDB_PositionalOption
                        {
                            Text = option.LeftText,
                            ImageUrl = option.LeftImageUrl
                        })))
                .ForMember(x => x.Right,
                    x => x.MapFrom(m =>
                        m.Select(option => new TestsDB_PositionalOption
                        {
                            Text = option.RightText,
                            ImageUrl = option.RightImageUrl
                        })));
            CreateMap<UserAPI_TestCommonOption_DTO, TestsDB_PositionalOption>();
            CreateMap<UserAPI_PostTestQuestion_DTO, TestsDB_Question_DTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            new TestsDB_OptionsData
                            {
                                Data = Mapper.Map<TestsDB_ChoiceOption[]>(m.Options)
                            }.ToBsonDocument(null, null, default) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<TestsDB_MatchingOptionsData>(m.Options).ToBsonDocument(null, null, default) :

                        m.Type == QuestionTypes.Sequence ?
                            new TestsDB_OptionsData
                            {
                                Data = Mapper.Map<TestsDB_PositionalOption[]>(m.Options)
                            }.ToBsonDocument(null, null, default) :

                        new BsonDocument()));
            CreateMap<KeyValuePair<string, UserAPI_PostTestQuestion_DTO>, KeyValuePair<string, TestsDB_Question_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, TestsDB_Question_DTO>(
                    x.Key,
                    Mapper.Map<TestsDB_Question_DTO>(x.Value)));
            CreateMap<UserAPI_PostTestSection_DTO, TestsDB_Section_DTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, TestsDB_Question_DTO>>(m.Questions)));
            #endregion

            CreateMap<UserAPI_PostTest_DTO, TestsDB_Test_DTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m =>
                        new Dictionary<string, UserAPI_PostTestSections_DTO>()));
            #endregion
        }
    }
}