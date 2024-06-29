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
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public PurchaseTicketUseCase(ITicketRepository ticketRepository, IMapper mapper, ITypeTicketRepository typeTicketRepository)
        {
            _ticketRepository = ticketRepository;
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
        }

        public Ticket Execute(int ticketId, string userId)
        {
            var ticketType = _typeTicketRepository.GetById(ticketId);

            if (ticketType.QuantityAvailable == 0)
            {
                //TODO
                return null;
            }

            if(ticketType == null)
            {


                public Ticket(Event ev, User user, TypeTicket typeTicket)

        //TODO: hacer un map
        var newTicket = new Ticket(ticketType.,  );

            ticket.QuantityAvailable -= 1;

            _typeTicketRepository.UpdateTicketQuantityAvailable(ticket);
            
            _ticketRepository.AddTicket(newTicket);

            return newTicket;
        }
    }
}
