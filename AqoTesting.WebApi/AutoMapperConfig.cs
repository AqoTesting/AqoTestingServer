using System;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Tests;
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
                cfg.CreateMap<SignUpUserDTO, User>()
                    .ForMember(x => x.PasswordHash,
                        x => x.MapFrom(m => Sha256.Compute(m.Password)))
                    .ForMember(x => x.RegistrationDate,
                        x => x.MapFrom(m => DateTime.UtcNow));

                cfg.CreateMap<User, AuthUser>()
                    .ForMember(x => x.Role,
                        x => x.MapFrom(m => Role.User));

                cfg.CreateMap<User, GetUserDTO>();

                // Rooms
                cfg.CreateMap<Room, GetUserRoomsItemDTO>();

                cfg.CreateMap<PostUserRoomDTO, Room>();
                
                cfg.CreateMap<UserRoomFieldDTO, RoomFieldInputData>();
                cfg.CreateMap<UserRoomFieldDTO, RoomFieldSelectData>();

                cfg.CreateMap<RoomField, UserRoomFieldDTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Input)
                            {
                                var value = m.Data.GetElement("Placeholder").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }))
                    .ForMember(x => x.Mask,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Input)
                            {
                                var value = m.Data.GetElement("Mask").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }))
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Select)
                            {
                                var value = m.Data.GetElement("Options").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }));

                cfg.CreateMap<RoomField, GetMemberRoomFieldDTO>()
                    .ForMember(x => x.Placeholder,
                        // Фу уродство, но пашет
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Input)
                            {
                                var value = m.Data.GetElement("Placeholder").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }))
                    .ForMember(x => x.Mask,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Input)
                            {
                                var value = m.Data.GetElement("Mask").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }))
                    .ForMember(x => x.Options,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Select)
                            {
                                var value = m.Data.GetElement("Options").Value;

                                return !(value is BsonNull) ? value : null;
                            }
                            else
                                return null;
                        }));

                cfg.CreateMap<Room, GetUserRoomDTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<UserRoomFieldDTO[]>(m.Fields)
                        ));

                cfg.CreateMap<Room, GetMemberRoomDTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<GetMemberRoomFieldDTO[]>(m.Fields)
                        ));

                cfg.CreateMap<UserRoomFieldDTO, RoomField>()
                    .ForMember(x => x.Data,
                        x => x.ResolveUsing(m => {
                            if (m.Type == FieldType.Input)
                                return Mapper.Map<RoomFieldInputData>(m).ToBsonDocument();

                            else if (m.Type == FieldType.Select)
                                return Mapper.Map<RoomFieldSelectData>(m).ToBsonDocument();

                            else
                                return new BsonDocument();
                        }));

                // Tests
                cfg.CreateMap<Test, GetTestsItemDTO>();
                cfg.CreateMap<Test, GetTestDTO>();
            });
        }
    }
}
