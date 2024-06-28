using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Domain.Entities;

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
            CreateMap<TypeTicket, TypeTicketDto>().ReverseMap();

            CreateMap<User, UserDto>();
        }
    }
}
