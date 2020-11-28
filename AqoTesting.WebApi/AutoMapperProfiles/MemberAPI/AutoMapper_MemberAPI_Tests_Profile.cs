using AqoTesting.Shared.DTOs.API.MemberAPI.Rooms;
using AqoTesting.Shared.DTOs.API.MemberAPI.Tests;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AutoMapper;
using System.Linq;

namespace AqoTesting.WebApi.AutoMapperProfiles.MemberAPI
{
    public class AutoMapper_MemberAPI_Tests_Profile : Profile
    {
        public AutoMapper_MemberAPI_Tests_Profile()
        {
            #region DB -> API
            CreateMap<TestsDB_Document_DTO, MemberAPI_TestDocument_DTO>();
            CreateMap<TestsDB_Rank_DTO, MemberAPI_TestRank_DTO>();
            CreateMap<TestsDB_Test_DTO, MemberAPI_GetTest_DTO>();
            #endregion

            #region API -> DB

            #endregion
        }
    }
}
