using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.DTOs.UsersDto;

using EventMS.Domain.Entities;
using EventMS.Infrastructure.Profiles;
using System.Security.Claims;

namespace EventManagementSystemAPI.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // create the profiles
            CreateMap<Event, EventDto>();
            CreateMap<EventDto, Event>();
            CreateMap<UpdateEventDto, Event>();
            CreateMap<ClaimsPrincipal, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.NameIdentifier) ?? src.FindFirstValue("sub")))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.GivenName)))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Surname) ?? src.FindFirstValue("family_name")))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Email)))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Name) ?? src.FindFirstValue("preferred_username")))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Role) ?? src.Claims.FirstOrDefault(c => c.Type == "realm_access").Value));
            //CreateMap<ClaimsPrincipal, UserDto>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.NameIdentifier).Value))
            //   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Name).Value))
            //   .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Surname).Value))
            //   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Email).Value))
            //   .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.FindFirst("nickname").Value))
            //   .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Role).Value));
        }
    }
}
