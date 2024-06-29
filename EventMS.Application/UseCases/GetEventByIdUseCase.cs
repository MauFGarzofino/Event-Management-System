using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
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
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
        }

        public EventDto Execute(int id)
        {
            var eventEntity = _eventRepository.GetEventById(id);
            if (eventEntity == null)
                throw new KeyNotFoundException($"Event with id '{id}' not found.");

            var eventDto = _mapper.Map<EventDto>(eventEntity);

            return eventDto;
        }
    }
}
