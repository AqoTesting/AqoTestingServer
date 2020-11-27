using AqoTesting.Shared.DTOs.API.UserAPI.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AutoMapper;
using MongoDB.Bson;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_Rooms_Profile : Profile
    {
        public AutoMapper_UserAPI_Rooms_Profile()
        {
            #region DB -> API
            CreateMap<RoomsDB_Field_DTO, UserAPI_RoomField_DTO>()
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
            CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoom_DTO>()
                .ForMember(x => x.Fields,
                    x => x.MapFrom(m =>
                        Mapper.Map<UserAPI_RoomField_DTO[]>(m.Fields)));

            CreateMap<RoomsDB_Room_DTO, UserAPI_GetRoomsItem_DTO>();
            #endregion

            #region API -> DB
            CreateMap<UserAPI_PostRoom_DTO, RoomsDB_Room_DTO>();

            CreateMap<UserAPI_RoomField_DTO, RoomsDB_InputField_DTO>();
            CreateMap<UserAPI_RoomField_DTO, RoomsDB_SelectField_DTO>();

            CreateMap<UserAPI_RoomField_DTO, RoomsDB_Field_DTO>()
                .ForMember(x => x.Data,
                    x => x.MapFrom(m =>
                        m.Type == FieldType.Input ?
                            Mapper.Map<RoomsDB_InputField_DTO>(m).ToBsonDocument(null, null, default) :
                        m.Type == FieldType.Select ?
                            Mapper.Map<RoomsDB_SelectField_DTO>(m).ToBsonDocument(null, null, default) :
                        new BsonDocument()));
            #endregion
        }
    }
}
