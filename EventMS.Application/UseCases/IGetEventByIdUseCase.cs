using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class GetEventByIdUseCase : IGetEventByIdUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetEventByIdUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public EventDetailDto GetEventById(int eventId)
        {
            var eventEntity = _eventRepository.GetEventById(eventId);
            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found");
            }

            return _mapper.Map<EventDetailDto>(eventEntity);
        }
    }

}
