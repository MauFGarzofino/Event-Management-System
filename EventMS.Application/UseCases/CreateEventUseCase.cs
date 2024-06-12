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

        public CreateEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void Execute(Event newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Title) || newEvent.Date == default || newEvent.Time == default || string.IsNullOrWhiteSpace(newEvent.Location))
            {
                throw new ArgumentException("Missing mandatory fields: title, date, time, location.");
            }

            if (_eventRepository.EventExists(newEvent.Title, newEvent.Date, newEvent.Location))
            {
                throw new InvalidOperationException("An event with the same title, date, and location already exists.");
            }

            _eventRepository.AddEvent(newEvent);
        }
    }
}

