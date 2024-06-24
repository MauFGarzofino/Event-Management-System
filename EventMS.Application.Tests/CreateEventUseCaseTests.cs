using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Moq;

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
            var newEvent = _mapper.Map<Event>(newEventDto);

            if (string.IsNullOrWhiteSpace(newEvent.Title) || newEvent.Date == default || newEvent.Time == default || string.IsNullOrWhiteSpace(newEvent.Location))
            {
                throw new ArgumentException("Missing mandatory fields: title, date, time, location.");
            }

            if (_eventRepository.EventExists(newEvent.Title, newEvent.Date, newEvent.Time, newEvent.Location))
            {
                throw new InvalidOperationException("An event with the same title, date, and location already exists.");
            }

            _eventRepository.AddEvent(newEvent);
            return newEvent;
        }
    }
}