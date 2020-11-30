using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.DB.Tests;
using AutoMapper;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_TestsProfile : Profile
    {
        public AutoMapper_MemberAPI_TestsProfile()
        {
            #region DB -> API
            CreateMap<TestsDB_DocumentDTO, MemberAPI_TestDocumentDTO>();
            CreateMap<TestsDB_RankDTO, MemberAPI_TestRankDTO>();
            CreateMap<TestsDB_TestDTO, MemberAPI_GetTestDTO>();
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
