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
    public class AutoMapper_UserAPI_Attempts_Profile : Profile
    {
        public AutoMapper_UserAPI_Attempts_Profile()
        {
            #region DB -> API
            #region AttemptsDB_Attempt_DTO -> UserAPI_GetAttempt_DTO
            CreateMap<AttemptsDB_ChoiceOption, UserAPI_AttemptCommonOption_DTO>();
            CreateMap<AttemptsDB_PositionalOption, UserAPI_AttemptCommonOption_DTO>();

            CreateMap<AttemptsDB_ChoiceOptions_Container, UserAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x =>
                    Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>(x.Options));
            CreateMap<AttemptsDB_MatchingOptions_Container, UserAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x =>
                {
                    var optionsLength = x.LeftSequence.Length;
                    var sequence = new UserAPI_AttemptCommonOption_DTO[optionsLength];
                    for(var i = 0; i < optionsLength; i++)
                        sequence[i] = new UserAPI_AttemptCommonOption_DTO {
                            LeftText = x.LeftSequence[i].Text,
                            LeftImageUrl = x.LeftSequence[i].ImageUrl,
                            RightText = x.RightSequence[i].Text,
                            RightImageUrl = x.RightSequence[i].ImageUrl
                        };

                    return sequence;
                });
            CreateMap<AttemptsDB_SequenceOptions_Container, UserAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x =>
                    Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>(x.Sequence));

            CreateMap<AttemptsDB_Question_DTO, UserAPI_GetAttemptQuestion_DTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_ChoiceOptions_Container>(m.Options, null)) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_MatchingOptions_Container>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_SequenceOptions_Container>(m.Options, null)) :

                        new UserAPI_AttemptCommonOption_DTO[0]
                    ));
            CreateMap<KeyValuePair<string, AttemptsDB_Question_DTO>, KeyValuePair<string, UserAPI_GetAttemptQuestion_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetAttemptQuestion_DTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetAttemptQuestion_DTO>(x.Value)));

            CreateMap<AttemptsDB_Section_DTO, UserAPI_GetAttemptSection_DTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, UserAPI_GetAttemptQuestion_DTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, AttemptsDB_Section_DTO>, KeyValuePair<string, UserAPI_GetAttemptSection_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, UserAPI_GetAttemptSection_DTO>(
                    x.Key,
                    Mapper.Map<UserAPI_GetAttemptSection_DTO>(x.Value)));

            CreateMap<AttemptsDB_Attempt_DTO, UserAPI_GetAttempt_DTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, UserAPI_GetAttemptSection_DTO>>(m.Sections)));
            #endregion

            CreateMap<AttemptsDB_Attempt_DTO, UserAPI_GetAttemptsItem_DTO>();
            #endregion

            #region API -> DB
            
            #endregion
        }
    }
}
