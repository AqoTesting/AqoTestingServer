using System;
using AqoTesting.Core.Utils;
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
                cfg.CreateMap<Room, GetRoomsItemDTO>();

                cfg.CreateMap<PostRoomDTO, Room>();

                cfg.CreateMap<RoomFieldDTO, RoomFieldInputData>();
                cfg.CreateMap<RoomFieldDTO, RoomFieldSelectData>();

                cfg.CreateMap<RoomField, RoomFieldDTO>()
                    .ForMember(x => x.Placeholder,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input ? m.Data.GetElement("Placeholder").Value : null
                        ))
                    .ForMember(x => x.Mask,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Input ? m.Data.GetElement("Mask").Value : null
                        ))
                    .ForMember(x => x.Options,
                        x => x.MapFrom(m =>
                            m.Type == FieldType.Select ? m.Data.GetElement("Options").Value : null
                        ));
                    //.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                cfg.CreateMap<Room, GetRoomDTO>()
                    .ForMember(x => x.Fields,
                        x => x.MapFrom(m =>
                            Mapper.Map<RoomFieldDTO[]>(m.Fields)
                        ));


                cfg.CreateMap<RoomFieldDTO, RoomField>()
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
