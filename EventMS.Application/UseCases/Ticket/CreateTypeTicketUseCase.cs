using AutoMapper;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases.Ticket
{
    public class CreateTypeTicketUseCase : ICreateTypeTicketUseCase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public CreateTypeTicketUseCase(ITypeTicketRepository typeTicketRepository, IEventRepository eventRepository, IMapper mapper)
        {
            _typeTicketRepository = typeTicketRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<TypeTicket> ExecuteAsync(TypeTicketDto typeTicketDto)
        {
            var eventEntity = _eventRepository.GetEventById(typeTicketDto.EventId);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with id '{typeTicketDto.EventId}' not found.");
            }

            var typeTicket = _mapper.Map<TypeTicket>(typeTicketDto);
            eventEntity.AddTypeTicket(typeTicket);

            await _typeTicketRepository.AddTypeTicketAsync(typeTicket);

            return typeTicket;
        }
    }
}
