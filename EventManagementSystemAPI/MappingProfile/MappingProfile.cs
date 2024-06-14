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
            CreateMap<EventDto, Event>();
            CreateMap<UpdateEventDto, Event>();

        }
    }
}
