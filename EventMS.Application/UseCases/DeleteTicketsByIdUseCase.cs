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
    public class DeleteTicketsByIdUseCase : IDeleteTicketsByIdUseCase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;

        public DeleteTicketsByIdUseCase(ITypeTicketRepository typeTicketRepository)
        {
            _typeTicketRepository = typeTicketRepository;
        }

        public void Execute(int eventId)
        {
            _typeTicketRepository.DeleteTicketsByEventId(eventId);
        }
    }
}
