using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_Members_Profile : Profile
    {
        public AutoMapper_MemberAPI_Members_Profile()
        {
            #region DB -> API
            CreateMap<MembersDB_Member_DTO, UserAPI_GetMembersItem_DTO>();
            #endregion

            #region API -> DB
            CreateMap<MemberAPI_SignUp_DTO, MembersDB_Member_DTO>()
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
