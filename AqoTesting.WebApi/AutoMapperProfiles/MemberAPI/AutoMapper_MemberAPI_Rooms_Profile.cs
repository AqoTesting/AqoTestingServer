using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AutoMapper;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_Rooms_Profile : Profile
    {
        public AutoMapper_MemberAPI_Rooms_Profile()
        {
            #region DB -> API
            CreateMap<RoomsDB_Field_DTO, MemberAPI_GetRoomField_DTO>()
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

            CreateMap<RoomsDB_Room_DTO, MemberAPI_GetRoom_DTO>()
                .ForMember(x => x.Fields,
                    x => x.MapFrom(m =>
                        Mapper.Map<MemberAPI_GetRoomField_DTO[]>(m.Fields)
                    ));
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
