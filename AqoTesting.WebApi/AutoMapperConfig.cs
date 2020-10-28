using System;
using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Users;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Tests;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Models;
using AutoMapper;

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
                cfg.CreateMap<Room, GetRoomDTO>();

                cfg.CreateMap<Room, GetRoomsItemDTO>();

                cfg.CreateMap<CreateRoomDTO, Room>();


                // Tests
                cfg.CreateMap<Test, GetTestsItemDTO>();
                cfg.CreateMap<Test, GetTestDTO>();
            });
        }
    }
}
