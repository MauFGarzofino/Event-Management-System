using AutoMapper;
using EventMS.Application.Ports;
using EventMS.Application.UseCases.UserUseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;

using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class PurchaseTicketUseCase : IPurchaseTicketUseCase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IMapper _mapper;

        public PurchaseTicketUseCase(ITicketRepository ticketRepository, IMapper mapper, ITypeTicketRepository typeTicketRepository, IEventRepository eventRepository, ICreateUserUseCase createUserUseCase)
        {
            _eventRepository = eventRepository;
            _ticketRepository = ticketRepository;
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
            _createUserUseCase = createUserUseCase;
        }

        public async Task<EventMS.Domain.Entities.Ticket> Execute(int ticketId, ClaimsPrincipal user, int evebtId) 
        {
            var userCreated = await _createUserUseCase.Execute(user);
                        
            TypeTicket ticketType =  await _typeTicketRepository.GetTypeTicketById(ticketId);

            
            if(ticketType == null)
            {
                throw new KeyNotFoundException($"Ticket Type with id '{ticketId}' not found.");
            }

            if (ticketType.QuantityAvailable == 0)
            {
                return null;
            }
           

            var newTicket = new EventMS.Domain.Entities.Ticket(evebtId, userCreated.Id, ticketId);

            ticketType.QuantityAvailable -= 1;

            await _typeTicketRepository.UpdateTypeTicket(ticketType);
            
            await _ticketRepository.AddTicket(newTicket);

            return newTicket;
        }
    }
}
