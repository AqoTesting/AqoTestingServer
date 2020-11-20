using System;
using System.Collections.Generic;
using System.Linq;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Questions;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.WebApi.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;

                #region Users
                cfg.CreateMap<UserAPI_SignUp_DTO, UsersDB_User_DTO>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m => Sha256.Compute(m.Password)))
                    .ForMember(x => x.RegistrationDate,
                        x => x.MapFrom(m => DateTime.UtcNow));

                cfg.CreateMap<UsersDB_User_DTO, AuthUser>()
                    .ForMember(x => x.Role,
                        x => x.MapFrom(m => Role.User));

                cfg.CreateMap<UsersDB_User_DTO, UserAPI_GetProfile_DTO>();
                #endregion

                #region Rooms
                cfg.CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoomsItem_DTO>();

                cfg.CreateMap<UserAPI_PostRoom_DTO, RoomsDB_Room_DTO>();

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_InputField_DTO>();
                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_SelectField_DTO>();

                cfg.CreateMap<RoomsDB_Field_DTO, UserAPI_RoomField_DTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Input) return null;
                            var placeholder = m.Data["Placeholder"];
                            return m.Type == FieldType.Input && placeholder.IsString ? placeholder.AsString : null;
                        }))
                    .ForMember(x => x.Mask,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Input) return null;
                            var mask = m.Data["Mask"];
                            return mask.IsString ? mask.AsString : null;
                        }))
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Select) return null;
                            var options = m.Data["Options"];
                            return options.IsBsonArray ? options.AsBsonArray.Select(item => item.AsString).ToArray() : null;
                        }));

                cfg.CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<UserAPI_RoomField_DTO[]>(m.Fields)
                        ));

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomsDB_Field_DTO>()
                    .ForMember(x => x.Data,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type == FieldType.Input)
                                return Mapper.Map<RoomsDB_InputField_DTO>(m).ToBsonDocument();

                            else if(m.Type == FieldType.Select)
                                return Mapper.Map<RoomsDB_SelectField_DTO>(m).ToBsonDocument();

                            else
                                return new BsonDocument();
                        }));

                cfg.CreateMap<RoomsDB_Field_DTO, MemberAPI_GetRoomField_DTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Input) return null;
                            var placeholder = m.Data["Placeholder"];
                            return m.Type == FieldType.Input && placeholder.IsString ? placeholder.AsString : null;
                        }))
                    .ForMember(x => x.Mask,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Input) return null;
                            var mask = m.Data["Mask"];
                            return mask.IsString ? mask.AsString : null;
                        }))
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type != FieldType.Select) return null;
                            var options = m.Data["Options"];
                            return options.IsBsonArray ? options.AsBsonArray.Select(item => item.AsString).ToArray() : null;
                        }));

                cfg.CreateMap<RoomsDB_Room_DTO, MemberAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<MemberAPI_GetRoomField_DTO[]>(m.Fields)
                        ));
                #endregion

                #region Tests
                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTestsItem_DTO>();
                cfg.CreateMap<TestsDB_Test_DTO, UserAPI_GetTest_DTO>();

                cfg.CreateMap<UserAPI_ChoiceOption_DTO, TestsDB_ChoiceOption_DTO>();
                cfg.CreateMap<UserAPI_MatchingOption_DTO, TestsDB_PositionalOption_DTO>();

                cfg.CreateMap<UserAPI_Question_DTO, TestsDB_Question_DTO>()
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                        {
                            if (m.Type == QuestionTypes.SingleChoice || m.Type == QuestionTypes.MultipleChoice)
                                return (new TestsDB_OptionsData_DTO {
                                  Data = Mapper.Map<TestsDB_ChoiceOption_DTO[]>(m.Options) }).ToBsonDocument();

                            if (m.Type == QuestionTypes.Matching)
                            {
                                var options = new Dictionary<TestsDB_PositionalOption_DTO, TestsDB_PositionalOption_DTO>();
                                foreach (var option in m.Options)
                                    options.Add(new TestsDB_PositionalOption_DTO
                                    {
                                        Text = option.LeftText,
                                        ImageUrl = option.LeftImageUrl
                                    }, new TestsDB_PositionalOption_DTO
                                    {
                                        Text = option.RightText,
                                        ImageUrl = option.RightImageUrl
                                    });

                                return options.ToBsonDocument();
                            }

                            if (m.Type == QuestionTypes.Sequence)
                                return Mapper.Map<TestsDB_PositionalOption_DTO[]>(m.Options).ToBsonDocument();

                            return new BsonDocument();
                        }));
                cfg.CreateMap<UserAPI_Section_DTO, TestsDB_Section_DTO>()
                    .ForMember(x => x.Questions,
                        x => x.MapFrom(m => Mapper.Map<TestsDB_Question_DTO[]>(m.Questions)));

                cfg.CreateMap<UserAPI_PostTest_DTO, TestsDB_Test_DTO>()
                    .ForMember(x => x.Sections,
                        x => x.MapFrom(m => Mapper.Map<TestsDB_Section_DTO[]>(m.Sections)));
                #endregion

                #region Members
                cfg.CreateMap<MembersDB_Member_DTO, UserAPI_GetMembersItem_DTO>();

                cfg.CreateMap<UserAPI_PostMember_DTO, MembersDB_Member_DTO>();

                cfg.CreateMap<MemberAPI_SignUp_DTO, MembersDB_Member_DTO>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m =>
                            Sha256.Compute(m.Password)    
                        ))
                    .ForMember(x => x.RoomId,
                        x => x.MapFrom(m =>
                            ObjectId.Parse(m.RoomId)
                        ));
                #endregion
            });
        }
    }
}
