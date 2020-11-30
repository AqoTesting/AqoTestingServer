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
    public class AutoMapper_CrossObjectsProfile : Profile
    {
        public AutoMapper_CrossObjectsProfile()
        {
            #region TestsDB_TestDTO -> AttemptsDB_AttemptDTO
            CreateMap<TestsDB_ChoiceOption, AttemptsDB_ChoiceOption>();
            CreateMap<TestsDB_PositionalOption[], AttemptsDB_PositionalOption[]>()
                .ConstructUsing(x => {
                    var attemptOptions = new AttemptsDB_PositionalOption[x.Length];
                    for(var i = 0; i < x.Length; i++)
                        attemptOptions[i] = new AttemptsDB_PositionalOption {
                            CorrectIndex = i,
                            Text = x[i].Text,
                            ImageUrl = x[i].ImageUrl };

                    return attemptOptions; });

            CreateMap<TestsDB_QuestionDTO, AttemptsDB_QuestionDTO>()
                .ForMember(x => x.Options,
                    x => x.ResolveUsing(m => {
                        switch(m.Type) {
                            case QuestionTypes.SingleChoice:
                            case QuestionTypes.MultipleChoice:
                                var choiceOptionsData = BsonSerializer.Deserialize<TestsDB_ChoiceOptionsContainer>(m.Options, null);
                                
                                return new AttemptsDB_ChoiceOptionsContainer {
                                    Options = Mapper.Map<AttemptsDB_ChoiceOption[]>(m.Shuffle.Value ?
                                        AttemptConstructor.ShuffleArray(choiceOptionsData.Options) :
                                    choiceOptionsData.Options)
                                }.ToBsonDocument();

                            case QuestionTypes.Matching:
                                var matchingOptionsData = BsonSerializer.Deserialize<TestsDB_MatchingOptionsContainer>(m.Options, null);

                                return new AttemptsDB_MatchingOptionsContainer {
                                    LeftSequence = AttemptConstructor.ShuffleArray(
                                        Mapper.Map<AttemptsDB_PositionalOption[]>(matchingOptionsData.LeftSequence)),
                                    RightSequence = AttemptConstructor.ShuffleArray(
                                        Mapper.Map<AttemptsDB_PositionalOption[]>(matchingOptionsData.RightSequence))
                                }.ToBsonDocument();

                            case QuestionTypes.Sequence:
                                var sequenceOptionsContainer = BsonSerializer.Deserialize<TestsDB_SequenceOptionsContainer>(m.Options, null);
                                
                                return new AttemptsDB_SequenceOptionsContainer {
                                    Sequence = AttemptConstructor.ShuffleArray(
                                        Mapper.Map<AttemptsDB_PositionalOption[]>(sequenceOptionsContainer.Sequence))
                                }.ToBsonDocument();

                            default:
                                return new BsonDocument();
                        }}));
            CreateMap<KeyValuePair<string, TestsDB_QuestionDTO>, KeyValuePair<string, AttemptsDB_QuestionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, AttemptsDB_QuestionDTO>(
                    x.Key,
                    Mapper.Map<AttemptsDB_QuestionDTO>(x.Value)));

            CreateMap<TestsDB_SectionDTO, AttemptsDB_SectionDTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, AttemptsDB_QuestionDTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, TestsDB_SectionDTO>, KeyValuePair<string, AttemptsDB_SectionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, AttemptsDB_SectionDTO>(
                    x.Key,
                    Mapper.Map<AttemptsDB_SectionDTO>(x.Value)));

            CreateMap<TestsDB_TestDTO, AttemptsDB_AttemptDTO>()
                .ForMember(x => x.Id,
                    x => x.Ignore())
                .ForMember(x => x.TestId,
                    x => x.MapFrom(m =>
                        m.Id))
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, AttemptsDB_SectionDTO>>(
                            AttemptConstructor.SelectSections(m))));
            #endregion
        }
    }
}
