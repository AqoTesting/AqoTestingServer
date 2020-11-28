using AqoTesting.Shared.DTOs.API.UserAPI.Attempts;
using AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
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
            CreateMap<AttemptsDB_MatchingOptions, UserAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x => {
                    var commonOptionDTOs = new UserAPI_AttemptCommonOption_DTO[x.Left.Length];
                    for (var i = 0; i < x.Left.Length; i++)
                        commonOptionDTOs[i] = new UserAPI_AttemptCommonOption_DTO {
                            LeftText = x.Left[i].Text,
                            LeftImageUrl = x.Left[i].ImageUrl,
                            RightText = x.Right[i].Text,
                            RightImageUrl = x.Right[i].ImageUrl };

                    return commonOptionDTOs;
                });
            CreateMap<AttemptsDB_PositionalOption, UserAPI_AttemptCommonOption_DTO>();

            CreateMap<AttemptsDB_Question_DTO, UserAPI_GetAttemptQuestion_DTO>()
                .ForMember(x => x.Options,
                    x => x.ResolveUsing(m => {
                        var optionsData = BsonSerializer.Deserialize<AttemptsDB_OptionsData>(m.Options, null);

                        return m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            new UserAPI_AttemptOptions_DTO {
                                Correct = null,
                                Answer = Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>((AttemptsDB_ChoiceOption[]) optionsData.Answer) } :

                        m.Type == QuestionTypes.Matching ?
                            new UserAPI_AttemptOptions_DTO {
                                Correct = Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>((AttemptsDB_MatchingOptions) optionsData.Correct),
                                Answer = Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>((AttemptsDB_MatchingOptions) optionsData.Answer) } :

                        m.Type == QuestionTypes.Sequence ?
                            new UserAPI_AttemptOptions_DTO {
                                Correct = Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>((AttemptsDB_PositionalOption) optionsData.Correct),
                                Answer = Mapper.Map<UserAPI_AttemptCommonOption_DTO[]>((AttemptsDB_PositionalOption) optionsData.Answer) } :

                        new UserAPI_AttemptOptions_DTO();
                    }));
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
