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
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void DeleteEvent(int id)
        {
            var existingEvent = _eventRepository.GetEventById(id);

            if (existingEvent == null)
            {
                throw new ArgumentException("Event not found");
            }

            _eventRepository.DeleteEvent(existingEvent);
        }
    }
}
