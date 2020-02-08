using AutoMapper;
using newNet.Models;
using newNet.Models.DTOs;

namespace newNet.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDTO, User>();
            CreateMap<User, UserDetailsDTO>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}