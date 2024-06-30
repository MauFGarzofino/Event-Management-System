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
    public class GetTicketTypeCountsUseCase : IGetTicketTypeCountsUseCase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;
        private readonly IMapper _mapper;

        public GetTicketTypeCountsUseCase(ITypeTicketRepository typeTicketRepository, IMapper mapper)
        {
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
        }

        public IEnumerable<TicketTypeCountDto> Execute(int eventId)
        {
            var ticketTypeCounts = _typeTicketRepository.GetTicketTypeCounts(eventId);
            return _mapper.Map<IEnumerable<TicketTypeCountDto>>(ticketTypeCounts);
        }
    }
}
