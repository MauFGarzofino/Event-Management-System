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

        public DeleteEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void Execute(int id)
        {
            _eventRepository.DeleteEvent(id);
        }
    }
}
