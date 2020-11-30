using AqoTesting.Shared.DTOs.API.UserAPI.Attempts;
using AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts.OptionsData;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_AttemptsProfile : Profile
    {
        public AutoMapper_UserAPI_AttemptsProfile()
        {
            #region DB -> API
            #region AttemptsDB_AttemptDTO -> UserAPI_GetAttemptDTO
            CreateMap<AttemptsDB_ChoiceOption, UserAPI_AttemptCommonOptionDTO>();
            CreateMap<AttemptsDB_PositionalOption, UserAPI_AttemptCommonOptionDTO>();

            CreateMap<AttemptsDB_ChoiceOptionsContainer, UserAPI_AttemptCommonOptionDTO[]>()
                .ConstructUsing(x =>
                    Mapper.Map<UserAPI_AttemptCommonOptionDTO[]>(x.Options));
            CreateMap<AttemptsDB_MatchingOptionsContainer, UserAPI_AttemptCommonOptionDTO[]>()
                .ConstructUsing(x =>
                {
                    var optionsLength = x.LeftSequence.Length;
                    var sequence = new UserAPI_AttemptCommonOptionDTO[optionsLength];
                    for(var i = 0; i < optionsLength; i++)
                        sequence[i] = new UserAPI_AttemptCommonOptionDTO {
                            LeftText = x.LeftSequence[i].Text,
                            LeftImageUrl = x.LeftSequence[i].ImageUrl,
                            RightText = x.RightSequence[i].Text,
                            RightImageUrl = x.RightSequence[i].ImageUrl
                        };

                    return sequence;
                });
            CreateMap<AttemptsDB_SequenceOptionsContainer, UserAPI_AttemptCommonOptionDTO[]>()
                .ConstructUsing(x =>
                    Mapper.Map<UserAPI_AttemptCommonOptionDTO[]>(x.Sequence));

            CreateMap<AttemptsDB_QuestionDTO, UserAPI_GetAttemptQuestionDTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<UserAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_ChoiceOptionsContainer>(m.Options, null)) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<UserAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_MatchingOptionsContainer>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<UserAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_SequenceOptionsContainer>(m.Options, null)) :

                        new UserAPI_AttemptCommonOptionDTO[0]
                    ));
            CreateMap<KeyValuePair<string, AttemptsDB_QuestionDTO>, KeyValuePair<string, UserAPI_GetAttemptQuestionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetAttemptQuestionDTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetAttemptQuestionDTO>(x.Value)));

            CreateMap<AttemptsDB_SectionDTO, UserAPI_GetAttemptSectionDTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, UserAPI_GetAttemptQuestionDTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, AttemptsDB_SectionDTO>, KeyValuePair<string, UserAPI_GetAttemptSectionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetAttemptSectionDTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetAttemptSectionDTO>(x.Value)));

            CreateMap<AttemptsDB_AttemptDTO, UserAPI_GetAttemptDTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetAttemptSectionDTO>>(m.Sections)));
            #endregion

            CreateMap<AttemptsDB_AttemptDTO, UserAPI_GetAttemptsItemDTO>();
            #endregion

            #region API -> DB
            
            #endregion
        }
    }
}
