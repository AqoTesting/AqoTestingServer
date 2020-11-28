using AqoTesting.WebApi.AutoMapperProfiles;
using AqoTesting.WebApi.AutoMapperProfiles.MemberAPI;
using AqoTesting.WebApi.AutoMapperProfiles.UserAPI;
using AutoMapper;

namespace AqoTesting.WebApi.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;

                cfg.AddProfile<AutoMapper_MemberAPI_Attempts_Profile>();
                cfg.AddProfile<AutoMapper_MemberAPI_Members_Profile>();
                cfg.AddProfile<AutoMapper_MemberAPI_Rooms_Profile>();

                cfg.AddProfile<AutoMapper_UserAPI_Attempts_Profile>();
                cfg.AddProfile<AutoMapper_UserAPI_Members_Profile>();
                cfg.AddProfile<AutoMapper_UserAPI_Rooms_Profile>();
                cfg.AddProfile<AutoMapper_UserAPI_Tests_Profile>();
                cfg.AddProfile<AutoMapper_UserAPI_Users_Profile>();

                cfg.AddProfile<AutoMapper_CrossObjects_Profile>();
            });
        }
    }
}
