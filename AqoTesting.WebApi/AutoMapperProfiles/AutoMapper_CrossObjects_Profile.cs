using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts.OptionsData;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Tests.Options;
using AqoTesting.Shared.DTOs.DB.Tests.OptionsContainers;
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

            CreateMap<TestsDB_Question_DTO, AttemptsDB_Question_DTO>()
                .ForMember(x => x.Options,
                    x => x.ResolveUsing(m => {
                        switch(m.Type) {
                            case QuestionTypes.SingleChoice:
                            case QuestionTypes.MultipleChoice:
                                var choiceOptionsData = BsonSerializer.Deserialize<TestsDB_ChoiceOptions_Container>(m.Options, null);
                                return new AttemptsDB_ChoiceOptions_Container {
                                    Options = Mapper.Map<AttemptsDB_ChoiceOption[]>(m.Shuffle.Value ?
                                        AttemptConstructor.ShuffleArray(choiceOptionsData.Options) :
                                    choiceOptionsData.Options)
                                }.ToBsonDocument();

                            case QuestionTypes.Matching:
                                var matchingOptionsData = BsonSerializer.Deserialize<TestsDB_MatchingOptions_Container>(m.Options, null);
                                return new AttemptsDB_MatchingOptions_Container {
                                    LeftCorrectSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(matchingOptionsData.LeftOptions),
                                    RightCorrectSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(matchingOptionsData.RightOptions),
                                    LeftAnswerSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(
                                        AttemptConstructor.ShuffleArray(matchingOptionsData.LeftOptions)),
                                    RightAnswerSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(
                                        AttemptConstructor.ShuffleArray(matchingOptionsData.RightOptions))
                                }.ToBsonDocument();

                            case QuestionTypes.Sequence:
                                var sequenceOptionsContainer = BsonSerializer.Deserialize<TestsDB_SequenceOptions_Container>(m.Options, null);
                                return new AttemptsDB_SequenceOptions_Container {
                                    CorrectSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(sequenceOptionsContainer.Options),
                                    AnswerSequence = Mapper.Map<AttemptsDB_PositionalOption[]>(
                                        AttemptConstructor.ShuffleArray(sequenceOptionsContainer.Options))
                                }.ToBsonDocument();

                            default:
                                return new BsonDocument();
                        }}));
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
