using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using EventMS.Application.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class CreateTicketUseCase: ICreateTicketUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public CreateTicketUseCase(ITicketRepository ticketRepository, IEventRepository eventRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public Ticket Execute(TicketDto ticketDto)
        {
            var eventExists = _eventRepository.GetEventById(ticketDto.EventId);
            if (eventExists == null)
            {
                throw new ArgumentException("Event not found.");
            }

            var newTicket = _mapper.Map<Ticket>(ticketDto);
            _ticketRepository.AddTicket(newTicket);
            return newTicket;
        }
    }
}
