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

        public GetEventByIdUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public EventDetailDto GetEventById(int eventId)
        {
            var eventEntity = _eventRepository.GetEventById(eventId);

            if (eventEntity == null)
            {
                throw new ArgumentException("Event not found");
            }

            return new EventDetailDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Date = eventEntity.Date,
                Time = eventEntity.Time,
                Location = eventEntity.Location
            };
        }
    }
}
