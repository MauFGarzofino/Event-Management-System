using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public UpdateEventUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public Event Execute(UpdateEventDto updateEVentDto)
        {
            var updateEvent = _mapper.Map<Event>(updateEVentDto);


            var existingEvent = _eventRepository.GetEventById(updateEvent.Id);
            if (existingEvent == null)
            {
                throw new ArgumentException("The event to be updated was not found.");
            }

            if (_eventRepository.EventExists(updateEvent.Title, updateEvent.Date, updateEvent.Location))
            {
                throw new InvalidOperationException("An event with the same title, date, and location already exists.");
            }

            _eventRepository.UpdateEvent(updateEvent);
            return updateEvent;
        }
    }
}
