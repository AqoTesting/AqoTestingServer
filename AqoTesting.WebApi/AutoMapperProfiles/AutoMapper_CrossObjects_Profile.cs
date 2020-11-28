using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Tests.Options;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;

namespace AqoTesting.WebApi.AutoMapperProfiles
{
    public class AutoMapper_CrossObjects_Profile : Profile
    {
        public AutoMapper_CrossObjects_Profile()
        {
            #region TestsDB_Test_DTO -> AttemptsDB_Attempt_DTO
            CreateMap<TestsDB_ChoiceOption, AttemptsDB_ChoiceOption>();
            CreateMap<TestsDB_PositionalOption, AttemptsDB_PositionalOption>();
            CreateMap<TestsDB_MatchingOptionsData, AttemptsDB_MatchingOptions>()
                .ForMember(x => x.Left,
                    x => x.MapFrom(m =>
                        Mapper.Map<AttemptsDB_PositionalOption>(m.Left)))
                .ForMember(x => x.Right,
                    x => x.MapFrom(m =>
                        Mapper.Map<AttemptsDB_PositionalOption>(m.Right)));

            CreateMap<TestsDB_Question_DTO, AttemptsDB_Question_DTO>()
                .ForMember(x => x.Options,
                    x => x.ResolveUsing(m =>
                    {
                        var optionsData = BsonSerializer.Deserialize<TestsDB_OptionsData>(m.Options, null);

                        return m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            new AttemptsDB_OptionsData {
                                Correct = null,
                                Answer = Mapper.Map<AttemptsDB_ChoiceOption[]>(
                                    m.Shuffle.Value ?
                                        AttemptConstructor.ShuffleArray((TestsDB_ChoiceOption[]) optionsData.Data) :
                                    (TestsDB_ChoiceOption[]) optionsData.Data)
                            }.ToBsonDocument() :

                        m.Type == QuestionTypes.Matching ?
                            new AttemptsDB_OptionsData
                            {
                                Correct = Mapper.Map<AttemptsDB_MatchingOptions>((TestsDB_MatchingOptionsData) optionsData.Data),
                                Answer = Mapper.Map<AttemptsDB_MatchingOptions>(
                                    AttemptConstructor.ShuffleMatchingOptions((TestsDB_MatchingOptionsData) optionsData.Data))
                            }.ToBsonDocument() :

                        m.Type == QuestionTypes.Sequence ?
                            new AttemptsDB_OptionsData
                            {
                                Correct = Mapper.Map<AttemptsDB_PositionalOption[]>((TestsDB_PositionalOption[]) optionsData.Data),
                                Answer = Mapper.Map<AttemptsDB_PositionalOption[]>(
                                    AttemptConstructor.ShuffleArray((TestsDB_PositionalOption[]) optionsData.Data))
                            }.ToBsonDocument() :

                        new BsonDocument();
                    }));
            CreateMap<KeyValuePair<string, TestsDB_Question_DTO>, KeyValuePair<string, AttemptsDB_Question_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, AttemptsDB_Question_DTO>(
                    x.Key,
                    Mapper.Map<AttemptsDB_Question_DTO>(x.Value)));

            CreateMap<TestsDB_Section_DTO, AttemptsDB_Section_DTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, AttemptsDB_Question_DTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, TestsDB_Section_DTO>, KeyValuePair<string, AttemptsDB_Section_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, AttemptsDB_Section_DTO>(
                    x.Key,
                    Mapper.Map<AttemptsDB_Section_DTO>(x.Value)));

            CreateMap<TestsDB_Test_DTO, AttemptsDB_Attempt_DTO>()
                .ForMember(x => x.TestId,
                    x => x.MapFrom(m =>
                        m.Id))
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, AttemptsDB_Section_DTO>>(
                            AttemptConstructor.SelectSections(m))));
            #endregion
        }
    }
}
