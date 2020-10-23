﻿using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AutoMapper;

namespace AqoTesting.WebApi.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Room, GetRoomsItemDTO>();
                cfg.CreateMap<CreateRoomDTO, Room>();
            });
        }
    }
}