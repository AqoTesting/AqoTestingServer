using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.DB.Users;
using AutoMapper;
using System;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_Users_Profile : Profile
    {
        public AutoMapper_UserAPI_Users_Profile()
        {
            #region DB -> API
            CreateMap<UsersDB_User_DTO, UserAPI_GetProfile_DTO>();
            #endregion

            #region API -> DB
            CreateMap<UserAPI_SignUp_DTO, UsersDB_User_DTO>()
                .ForMember(x => x.PasswordHash,
                    x => x.MapFrom(m => Sha256.Compute(m.Password)))
                .ForMember(x => x.RegistrationDate,
                    x => x.MapFrom(m => DateTime.UtcNow));
            #endregion
        }
    }
}
