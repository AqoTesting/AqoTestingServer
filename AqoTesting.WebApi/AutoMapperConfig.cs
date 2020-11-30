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

                cfg.AddProfile<AutoMapper_MemberAPI_AttemptsProfile>();
                cfg.AddProfile<AutoMapper_MemberAPI_MembersProfile>();
                cfg.AddProfile<AutoMapper_MemberAPI_RoomsProfile>();

                cfg.AddProfile<AutoMapper_UserAPI_AttemptsProfile>();
                cfg.AddProfile<AutoMapper_UserAPI_MembersProfile>();
                cfg.AddProfile<AutoMapper_UserAPI_RoomsProfile>();
                cfg.AddProfile<AutoMapper_UserAPI_TestsProfile>();
                cfg.AddProfile<AutoMapper_UserAPI_UsersProfile>();

                cfg.AddProfile<AutoMapper_CrossObjectsProfile>();
            });
        }
    }
}
