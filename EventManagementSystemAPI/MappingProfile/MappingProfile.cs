using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.DTOs.UsersDto;

using EventMS.Domain.Entities;
using System.Security.Claims;

namespace EventManagementSystemAPI.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<UpdateEventDto, Event>().ReverseMap();

            CreateMap<ClaimsPrincipal, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.NameIdentifier) ?? src.FindFirstValue("sub")))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.GivenName)))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Surname) ?? src.FindFirstValue("family_name")))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Email)))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Name) ?? src.FindFirstValue("preferred_username")))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.FindFirstValue(ClaimTypes.Role) ?? src.Claims.FirstOrDefault(c => c.Type == "realm_access").Value));

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<TypeTicket, TypeTicketDto>().ReverseMap();

            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.TypeTicket.Event.Title))
                .ForMember(dest => dest.TypeTicketName, opt => opt.MapFrom(src => src.TypeTicket.Name));

            CreateMap<TypeTicketCount, TicketTypeCountDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.TypeName));

            CreateMap<TypeTicketCount, TicketTypeCountDto>();
            CreateMap<EventRegistrationDto, EventRegistration>();
        }
    }
}
