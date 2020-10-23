using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AutoMapper;

namespace AqoTesting.WebApi.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Room, GetRoomDTO>();
                cfg.CreateMap<Room, GetRoomsItemDTO>();
                cfg.CreateMap<CreateRoomDTO, Room>();
            });
        }
    }
}
