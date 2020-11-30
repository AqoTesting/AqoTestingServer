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
    public class AutoMapper_MemberAPI_AttemptsProfile : Profile
    {
        public AutoMapper_MemberAPI_AttemptsProfile()
        {
            #region DB -> API
            #region AttemptsDB -> MemberAPI_GetAttemptDTO
            CreateMap<AttemptsDB_ChoiceOption, MemberAPI_AttemptCommonOptionDTO>();
            CreateMap<AttemptsDB_MatchingOptionsContainer, MemberAPI_AttemptCommonOptionDTO[]>()
                .ConstructUsing(x => {
                    var optionsLength = x.LeftSequence.Length;

                    var commonOptionDTOs = new MemberAPI_AttemptCommonOptionDTO[optionsLength];
                    for (var i = 0; i < optionsLength; i++)
                        commonOptionDTOs[i] = new MemberAPI_AttemptCommonOptionDTO
                        {
                            LeftText = x.LeftSequence[i].Text,
                            LeftImageUrl = x.LeftSequence[i].ImageUrl,
                            RightText = x.RightSequence[i].Text,
                            RightImageUrl = x.RightSequence[i].ImageUrl
                        };

                    return commonOptionDTOs;
                });
            CreateMap<AttemptsDB_PositionalOption, MemberAPI_AttemptCommonOptionDTO>();

            CreateMap<AttemptsDB_QuestionDTO, MemberAPI_GetAttemptQuestionDTO>()
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice ?
                            Mapper.Map<MemberAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_ChoiceOptionsContainer>(m.Options, null).Options) :

                        m.Type == QuestionTypes.Matching ?
                            Mapper.Map<MemberAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_MatchingOptionsContainer>(m.Options, null)) :

                        m.Type == QuestionTypes.Sequence ?
                            Mapper.Map<MemberAPI_AttemptCommonOptionDTO[]>(
                                BsonSerializer.Deserialize<AttemptsDB_SequenceOptionsContainer>(m.Options, null).Sequence) :

                        new MemberAPI_AttemptCommonOptionDTO[0]));
            CreateMap<KeyValuePair<string, AttemptsDB_QuestionDTO>, KeyValuePair<string, MemberAPI_GetAttemptQuestionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, MemberAPI_GetAttemptQuestionDTO>(
                    x.Key,
                    Mapper.Map<MemberAPI_GetAttemptQuestionDTO>(x.Value)));

            CreateMap<AttemptsDB_SectionDTO, MemberAPI_GetAttemptSectionDTO>()
                .ForMember(x => x.Questions,
                    x => x.MapFrom(m =>
                        Mapper.Map<Dictionary<string, MemberAPI_GetAttemptQuestionDTO>>(m.Questions)));
            CreateMap<KeyValuePair<string, AttemptsDB_SectionDTO>, KeyValuePair<string, MemberAPI_GetAttemptSectionDTO>>()
                .ConstructUsing(x => new KeyValuePair<string, MemberAPI_GetAttemptSectionDTO>(
                    x.Key,
                    Mapper.Map<MemberAPI_GetAttemptSectionDTO>(x.Value)));

            CreateMap<AttemptsDB_AttemptDTO, MemberAPI_GetAttemptDTO>()
                .ForMember(x => x.Sections,
                    x => x.MapFrom(m => Mapper.Map<Dictionary<string, MemberAPI_GetAttemptSectionDTO>>(m.Sections)));
            #endregion

            CreateMap<AttemptsDB_AttemptDTO, MemberAPI_ActiveAttemptResumeDataDTO>();
            CreateMap<AttemptsDB_AttemptDTO, MemberAPI_GetAttemptsItemDTO>();
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
