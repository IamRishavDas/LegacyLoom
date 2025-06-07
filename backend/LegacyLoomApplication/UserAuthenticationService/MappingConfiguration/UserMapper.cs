using AutoMapper;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Models;

namespace UserAuthenticationService.MappingConfiguration
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, src => src.MapFrom(src => src.Role.ToString()));
        }
    }
}
