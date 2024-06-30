using AutoMapper;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class PurchaseTicketUseCase : IPurchaseTicketUseCase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public PurchaseTicketUseCase(ITicketRepository ticketRepository, IMapper mapper, ITypeTicketRepository typeTicketRepository, IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
        }

        public Ticket Execute(int ticketId, User user) 
        {
            TypeTicket ticketType = _typeTicketRepository.GetTypeTicketById(ticketId);

            Event Event = _eventRepository.GetEventById(ticketType.EventId);

            if (ticketType.QuantityAvailable == 0)
            {
                //TODO
                return null;
            }

            if (ticketType == null)
            {

            }

            var newTicket = new Ticket(Event, user, ticketType);

            ticketType.QuantityAvailable -= 1;

            _typeTicketRepository.UpdateTypeTicket(ticketType);
            
            _ticketRepository.AddTicket(newTicket);

            return newTicket;
        }
    }
}
