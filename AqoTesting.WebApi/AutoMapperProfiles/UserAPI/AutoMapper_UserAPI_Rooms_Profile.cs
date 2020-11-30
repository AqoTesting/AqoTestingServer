using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_RoomsProfile : Profile
    {
        public AutoMapper_UserAPI_RoomsProfile()
        {
            #region DB -> API
            CreateMap<RoomsDB_FieldDTO, UserAPI_RoomFieldDTO>()
                .ForMember(x => x.Placeholder,
                    x => x.MapFrom(m =>
                        m.Type == FieldType.Input && m.Data["Placeholder"].IsString ?
                            m.Data["Placeholder"].AsString :
                        null))
                .ForMember(x => x.Mask,
                    x => x.MapFrom(m =>
                        m.Type == FieldType.Input && m.Data["Mask"].IsString ?
                            m.Data["Mask"].AsString :
                        null))
                .ForMember(x => x.Options,
                    x => x.MapFrom(m =>
                        m.Type == FieldType.Select && m.Data["Options"].IsBsonArray ?
                            m.Data["Options"].AsBsonArray.Select(item => item.AsString).ToArray() :
                        null));
            CreateMap<RoomsDB_RoomDTO, UserAPI_GetRoomDTO>()
                .ForMember(x => x.Fields,
                    x => x.MapFrom(m =>
                        Mapper.Map<UserAPI_RoomFieldDTO[]>(m.Fields)));

            CreateMap<RoomsDB_RoomDTO, UserAPI_GetRoomsItemDTO>();
            #endregion

            #region API -> DB
            CreateMap<UserAPI_PostRoomDTO, RoomsDB_RoomDTO>();

            CreateMap<UserAPI_RoomFieldDTO, RoomsDB_InputFieldDTO>();
            CreateMap<UserAPI_RoomFieldDTO, RoomsDB_SelectFieldDTO>();

            CreateMap<UserAPI_RoomFieldDTO, RoomsDB_FieldDTO>()
                .ForMember(x => x.Data,
                    x => x.MapFrom(m =>
                        m.Type == FieldType.Input ?
                            Mapper.Map<RoomsDB_InputFieldDTO>(m).ToBsonDocument(null, null, default) :
                        m.Type == FieldType.Select ?
                            Mapper.Map<RoomsDB_SelectFieldDTO>(m).ToBsonDocument(null, null, default) :
                        new BsonDocument()));
            #endregion
        }
    }
}
