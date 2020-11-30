using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.UserAPI.Account;
using AqoTesting.Shared.DTOs.DB.Users;
using AutoMapper;
using System;

namespace AqoTesting.WebApi.AutoMapperProfiles.UserAPI
{
    public class AutoMapper_UserAPI_UsersProfile : Profile
    {
        public AutoMapper_UserAPI_UsersProfile()
        {
            #region DB -> API
            CreateMap<UsersDB_UserDTO, UserAPI_GetProfileDTO>();
            #endregion

            #region API -> DB
            CreateMap<UserAPI_SignUpDTO, UsersDB_UserDTO>()
                .ForMember(x => x.PasswordHash,
                    x => x.MapFrom(m => Sha256.Compute(m.Password)))
                .ForMember(x => x.RegistrationDate,
                    x => x.MapFrom(m => DateTime.UtcNow));
            #endregion
        }
    }
}
