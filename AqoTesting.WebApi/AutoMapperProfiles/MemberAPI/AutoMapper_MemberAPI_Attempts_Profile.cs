using AqoTesting.Shared.DTOs.API.MemberAPI.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Attempts.Options;
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
            CreateMap<AttemptsDB_MatchingOptions, MemberAPI_AttemptCommonOption_DTO[]>()
                .ConstructUsing(x => {
                    var commonOptionDTOs = new MemberAPI_AttemptCommonOption_DTO[x.Left.Length];
                    for (var i = 0; i < x.Left.Length; i++)
                        commonOptionDTOs[i] = new MemberAPI_AttemptCommonOption_DTO
                        {
                            LeftText = x.Left[i].Text,
                            LeftImageUrl = x.Left[i].ImageUrl,
                            RightText = x.Right[i].Text,
                            RightImageUrl = x.Right[i].ImageUrl
                        };

                    return commonOptionDTOs;
                });
            CreateMap<AttemptsDB_PositionalOption, MemberAPI_AttemptCommonOption_DTO>();

            CreateMap<AttemptsDB_Question_DTO, MemberAPI_GetAttemptQuestion_DTO>()
                .ForMember(x => x.Options,
                    x => x.ResolveUsing(m => {
                        var optionsData = BsonSerializer.Deserialize<AttemptsDB_OptionsData>(m.Options, null);

                        return m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>((AttemptsDB_ChoiceOption[])optionsData.Answer) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>((AttemptsDB_MatchingOptions)optionsData.Answer) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<MemberAPI_AttemptCommonOption_DTO[]>((AttemptsDB_PositionalOption[])optionsData.Answer) :

                        new MemberAPI_AttemptCommonOption_DTO[0];
                    }));
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
