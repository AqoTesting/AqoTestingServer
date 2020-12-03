using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Tests.Options;
using AqoTesting.Shared.DTOs.DB.Tests.OptionsContainers;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_TestsProfile : Profile
    {
        public AutoMapper_UserAPI_TestsProfile()
        {
            #region DB -> API
            #region TestsDB_TestDTO -> UserAPI_GetTestDTO
            CreateMap<TestsDB_ChoiceOption, UserAPI_TestCommonOptionDTO>();
            CreateMap<TestsDB_MatchingOptionsContainer, UserAPI_TestCommonOptionDTO[]>()
                .ConstructUsing(x => {
                    var commonOptionDTOs = new UserAPI_TestCommonOptionDTO[x.LeftSequence.Length];
                    for (var i = 0; i < x.LeftSequence.Length; i++)
                        commonOptionDTOs[i] = new UserAPI_TestCommonOptionDTO {
                            LeftText = x.LeftSequence[i].Text,
                            LeftImageUrl = x.LeftSequence[i].ImageUrl,
                            RightText = x.RightSequence[i].Text,
                            RightImageUrl = x.RightSequence[i].ImageUrl };

                    return commonOptionDTOs;
                });
            CreateMap<TestsDB_PositionalOption, UserAPI_TestCommonOptionDTO>();
            CreateMap<TestsDB_FillInOption, UserAPI_TestCommonOptionDTO>();

            CreateMap<TestsDB_QuestionDTO, UserAPI_GetTestQuestionDTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<UserAPI_TestCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<TestsDB_ChoiceOptionsContainer>(m.Options, null).Options) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<UserAPI_TestCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<TestsDB_MatchingOptionsContainer>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<UserAPI_TestCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<TestsDB_SequenceOptionsContainer>(m.Options, null).Sequence) :

                        m.Type == QuestionTypes.FillIn ?
                            Mapper.Map<UserAPI_TestCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<TestsDB_FillInOptionsContainer>(m.Options, null).Options) :

                        new UserAPI_TestCommonOptionDTO[0]));
            CreateMap<KeyValuePair<string, TestsDB_QuestionDTO>, KeyValuePair<string, UserAPI_GetTestQuestionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetTestQuestionDTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetTestQuestionDTO>(x.Value)));

            CreateMap<TestsDB_SectionDTO, UserAPI_GetTestSectionDTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetTestQuestionDTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, TestsDB_SectionDTO>, KeyValuePair<string, UserAPI_GetTestSectionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetTestSectionDTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetTestSectionDTO>(x.Value)));
            CreateMap<TestsDB_TestDTO, UserAPI_GetTestDTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetTestSectionDTO>>(m.Sections)));
            #endregion

            CreateMap<TestsDB_TestDTO, UserAPI_GetTestsItemDTO>();
            #endregion

            #region API -> DB
            #region UserAPI_PostSectionDTO -> TestsDB_SectionDTO
            CreateMap<UserAPI_TestCommonOptionDTO, TestsDB_ChoiceOption>();
            CreateMap<UserAPI_TestCommonOptionDTO[], TestsDB_MatchingOptionsContainer>()
                .ForMember(x => x.LeftSequence,
                    x => x.MapFrom(m =>
                        m.Select(option => new TestsDB_PositionalOption
                        {
                            Text = option.LeftText,
                            ImageUrl = option.LeftImageUrl
                        })))
                .ForMember(x => x.RightSequence,
                    x => x.MapFrom(m =>
                        m.Select(option => new TestsDB_PositionalOption
                        {
                            Text = option.RightText,
                            ImageUrl = option.RightImageUrl
                        })));
            CreateMap<UserAPI_TestCommonOptionDTO, TestsDB_PositionalOption>();
            CreateMap<UserAPI_PostTestQuestionDTO, TestsDB_QuestionDTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            new TestsDB_ChoiceOptionsContainer
                            {
                                Options = Mapper.Map<TestsDB_ChoiceOption[]>(m.Options)
                            }.ToBsonDocument(null, null, default) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<TestsDB_MatchingOptionsContainer>(m.Options).ToBsonDocument(null, null, default) :

                        m.Type == QuestionTypes.Sequence ?
                            new TestsDB_SequenceOptionsContainer
                            {
                                Sequence = Mapper.Map<TestsDB_PositionalOption[]>(m.Options)
                            }.ToBsonDocument(null, null, default) :

                        m.Type == QuestionTypes.FillIn ?
                            new TestsDB_FillInOptionsContainer { 
                                Options = Mapper.Map<TestsDB_FillInOption[]>(m.Options)
                            }.ToBsonDocument(null, null, default) :

                        new BsonDocument()));
            CreateMap<KeyValuePair<string, UserAPI_PostTestQuestionDTO>, KeyValuePair<string, TestsDB_QuestionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, TestsDB_QuestionDTO>(
                    x.Key,
                    Mapper.Map<TestsDB_QuestionDTO>(x.Value)));
            CreateMap<UserAPI_PostTestSectionDTO, TestsDB_SectionDTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, TestsDB_QuestionDTO>>(m.Questions)));
            #endregion

            CreateMap<UserAPI_PostTestDTO, TestsDB_TestDTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m =>
                        new Dictionary<string, UserAPI_PostTestSectionsDTO>()));
            #endregion
        }
    }
}
