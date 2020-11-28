using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts.OptionsData;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_Attempts_Profile : Profile
    {
        public AutoMapper_MemberAPI_Attempts_Profile()
        {
            #region DB -> API
            #region AttemptsDB -> MemberAPI_GetAttempt_DTO
            CreateMap<AttemptsDB_ChoiceOption, MemberAPI_AttemptCommonOption_DTO>();
            CreateMap<AttemptsDB_MatchingOptions_Container, MemberAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x => {
                    var optionsLength = x.LeftCorrectSequence.Length;

                    var commonOptionDTOs = new MemberAPI_AttemptCommonOption_DTO[optionsLength];
                    for (var i = 0; i < optionsLength; i++)
                        commonOptionDTOs[i] = new MemberAPI_AttemptCommonOption_DTO
                        {
                            LeftText = x.LeftAnswerSequence[i].Text,
                            LeftImageUrl = x.LeftAnswerSequence[i].ImageUrl,
                            RightText = x.RightAnswerSequence[i].Text,
                            RightImageUrl = x.RightAnswerSequence[i].ImageUrl
                        };

                    return commonOptionDTOs;
                });
            CreateMap<AttemptsDB_PositionalOption, MemberAPI_AttemptCommonOption_DTO>();

            CreateMap<AttemptsDB_Question_DTO, MemberAPI_GetAttemptQuestion_DTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_ChoiceOptions_Container>(m.Options, null).Options) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_MatchingOptions_Container>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_SequenceOptions_Container>(m.Options, null).AnswerSequence) :

                        new MemberAPI_AttemptCommonOption_DTO[0]));
            CreateMap<KeyValuePair<string, AttemptsDB_Question_DTO>, KeyValuePair<string, MemberAPI_GetAttemptQuestion_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, MemberAPI_GetAttemptQuestion_DTO>(
                    x.Key,
                    Mapper.Map<MemberAPI_GetAttemptQuestion_DTO>(x.Value)));

            CreateMap<AttemptsDB_Section_DTO, MemberAPI_GetAttemptSection_DTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, MemberAPI_GetAttemptQuestion_DTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, AttemptsDB_Section_DTO>, KeyValuePair<string, MemberAPI_GetAttemptSection_DTO>>()
                .ConstructUsing(x => new KeyValuePair<string, MemberAPI_GetAttemptSection_DTO>(
                    x.Key,
                    Mapper.Map<MemberAPI_GetAttemptSection_DTO>(x.Value)));

            CreateMap<AttemptsDB_Attempt_DTO, MemberAPI_GetAttempt_DTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, MemberAPI_GetAttemptSection_DTO>>(m.Sections)));
            #endregion

            CreateMap<AttemptsDB_Attempt_DTO, MemberAPI_GetAttemptsItem_DTO>();
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
