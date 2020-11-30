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

            #endregion

            #region API -> DB
            CreateMap<UserAPI_PostMemberDTO, MembersDB_MemberDTO>();
            #endregion
        }
    }
}
