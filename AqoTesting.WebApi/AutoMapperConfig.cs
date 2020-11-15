using System;
using System.Linq;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
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

                // Users
                cfg.CreateMap<UserAPI_SignUp_DTO, User>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m => Sha256.Compute(m.Password)))
                    .ForMember(x => x.RegistrationDate,
                        x => x.MapFrom(m => DateTime.UtcNow));

                cfg.CreateMap<User, AuthUser>()
                    .ForMember(x => x.Role,
                        x => x.MapFrom(m => Role.User));

                cfg.CreateMap<User, UserAPI_GetProfile_DTO>();

                // Rooms
                cfg.CreateMap<Room, UserAPI_GetRoomsItem_DTO>();

                cfg.CreateMap<UserAPI_PostRoom_DTO, Room>();

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomFieldInputData>();
                cfg.CreateMap<UserAPI_RoomField_DTO, RoomFieldSelectData>();

                cfg.CreateMap<RoomField, UserAPI_RoomField_DTO>()
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

                cfg.CreateMap<RoomField, MemberAPI_GetRoomField_DTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input ? m.Data["Placeholder"].AsString : null
                        ))
                    .ForMember(x => x.Mask,
                        x => x.ResolveUsing(m =>
                            m.Type == FieldType.Input ? m.Data["Mask"].AsString : null
                        ))
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m =>
                            m.Type == FieldType.Select ? m.Data["Options"].AsBsonArray.Select(item => item.AsString).ToArray() : null
                        ));

                cfg.CreateMap<Room, UserAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<UserAPI_RoomField_DTO[]>(m.Fields)
                        ));

                cfg.CreateMap<Room, MemberAPI_GetRoom_DTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<MemberAPI_GetRoomField_DTO[]>(m.Fields)
                        ));

                cfg.CreateMap<UserAPI_RoomField_DTO, RoomField>()
                    .ForMember(x => x.Data,
                        x => x.ResolveUsing(m =>
                        {
                            if(m.Type == FieldType.Input)
                                return Mapper.Map<RoomFieldInputData>(m).ToBsonDocument();

                            else if(m.Type == FieldType.Select)
                                return Mapper.Map<RoomFieldSelectData>(m).ToBsonDocument();

                            else
                                return new BsonDocument();
                        }));

                // Tests
                cfg.CreateMap<Test, UserAPI_GetTestsItem_DTO>();
                cfg.CreateMap<Test, UserAPI_GetTest_DTO>();

                // Members
                cfg.CreateMap<Member, UserAPI_GetMembersItem_DTO>();

                cfg.CreateMap<UserAPI_PostMember_DTO, Member>()
                    .ForMember(x => x.IsChecked,
                        x => x.MapFrom(m => true));
            });
        }
    }
}
