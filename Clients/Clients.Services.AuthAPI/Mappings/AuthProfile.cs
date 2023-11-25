using AutoMapper;
using Clients.Data.Models;
using Clients.Services.AuthAPI.Models.Dto;
using Clients.Services.Business.Models.Auth;

namespace Clients.Services.AuthAPI.Mappings
{
    public class AuthProfile: Profile
    {
        public AuthProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserRegistrationDto, User>();
            CreateMap<LoginDto, Login>();
            CreateMap<LoginResponse, LoginResponseDto>();
        }
    }
}
