using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AutoMapper;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_MembersProfile : Profile
    {
        public AutoMapper_UserAPI_MembersProfile()
        {
            #region DB -> API
            CreateMap<MembersDB_MemberDTO, UserAPI_GetMemberDTO>();
            CreateMap<MembersDB_MemberDTO, UserAPI_GetMembersItemDTO>();
            #endregion

            #region API -> DB
            CreateMap<UserAPI_PostMemberDTO, MembersDB_MemberDTO>();
            #endregion
        }
    }
}
