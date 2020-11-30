using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Enums;
using AutoMapper;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_RoomsProfile : Profile
    {
        public AutoMapper_MemberAPI_RoomsProfile()
        {
            #region DB -> API
            CreateMap<RoomsDB_FieldDTO, MemberAPI_GetRoomFieldDTO>()
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

            CreateMap<RoomsDB_RoomDTO, MemberAPI_GetRoomDTO>()
                .ForMember(x => x.Fields,
                    x => x.MapFrom(m =>
                        Mapper.Map<MemberAPI_GetRoomFieldDTO[]>(m.Fields)
                    ));
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
