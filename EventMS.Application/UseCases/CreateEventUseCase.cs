using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public CreateEventUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public Event Execute(EventDto newEventDto)
        {
            if (string.IsNullOrWhiteSpace(newEventDto.Title) || newEventDto.Date == default || newEventDto.Time == default || string.IsNullOrWhiteSpace(newEventDto.Location))
            {
                throw new ArgumentException("Missing mandatory fields: title, date, time, location.");
            }

            var newEvent = _mapper.Map<Event>(newEventDto);

            if (_eventRepository.EventExists(newEvent.Title, newEvent.Date, newEvent.Time, newEvent.Location))
            {
                throw new InvalidOperationException("An event with the same title, date, and location already exists.");
            }

            _eventRepository.AddEvent(newEvent);
            return newEvent;
        }
    }
}

