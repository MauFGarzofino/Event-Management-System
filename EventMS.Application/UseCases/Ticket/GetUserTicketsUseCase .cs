using AutoMapper;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases.Ticket
{
    public class GetUserTicketsUseCase : IGetUserTicketsUseCase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetUserTicketsUseCase(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDto>> Execute(string userId)
        {
            var tickets = await _ticketRepository.GetTicketsByUserId(userId);
            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }
    }
}
