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
    public class GetEventsByDateUseCase : IGetEventsByDateUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetEventsByDateUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public IEnumerable<EventDto> Execute(DateTime date)
        {
            var events = _eventRepository.GetEventsByDate(date);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}



