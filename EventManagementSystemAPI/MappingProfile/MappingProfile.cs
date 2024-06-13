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
            CreateMap<EventDto, Event>().
                ForMember(d => d.Title, o => o.MapFrom( s => s.Title));
        }
    }
}
