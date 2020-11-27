using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AutoMapper;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_Members_Profile : Profile
    {
        public AutoMapper_UserAPI_Members_Profile()
        {
            #region DB -> API

            #endregion

            #region API -> DB
            CreateMap<UserAPI_PostMember_DTO, MembersDB_Member_DTO>();
            #endregion
        }
    }
}
