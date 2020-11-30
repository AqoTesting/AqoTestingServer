using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_MembersProfile : Profile
    {
        public AutoMapper_MemberAPI_MembersProfile()
        {
            #region DB -> API
            CreateMap<MembersDB_MemberDTO, UserAPI_GetMembersItemDTO>();
            #endregion

            #region API -> DB
            CreateMap<MemberAPI_SignUpDTO, MembersDB_MemberDTO>()
                .ForMember(x => x.PasswordHash,
                    x => x.MapFrom(m =>
                        Sha256.Compute(m.Password)))
                .ForMember(x => x.RoomId,
                    x => x.MapFrom(m =>
                        ObjectId.Parse(m.RoomId)));
            #endregion
        }
    }
}
