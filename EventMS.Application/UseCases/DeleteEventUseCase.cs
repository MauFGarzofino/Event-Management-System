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
        private readonly IMapper _mapper;

        public DeleteEventUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public void Execute(DeleteEventDto eventDto)
        {
            var eventToDelete = _eventRepository.GetEventByTitle(eventDto.Title);

            if (eventToDelete == null)
            {
                throw new KeyNotFoundException($"Event with title '{eventDto.Title}' not found.");
            }

            _eventRepository.DeleteEvent(eventToDelete.Id);
        }
    }
}

