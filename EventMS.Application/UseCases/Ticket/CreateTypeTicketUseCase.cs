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
        private readonly IMapper _mapper;

        public CreateTypeTicketUseCase(ITypeTicketRepository typeTicketRepository, IMapper mapper)
        {
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
        }

        public TypeTicket Execute(TypeTicketDto typeTicketDto)
        {
            var typeTicket = _mapper.Map<TypeTicket>(typeTicketDto);
            _typeTicketRepository.AddTypeTicket(typeTicket);
            return typeTicket;
        }
    }
}
