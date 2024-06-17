using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class GetAllEventsUseCase : IGetAllEventsUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GetAllEventsUseCase(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public IEnumerable<EventDto> Execute()
        {
            var events = _eventRepository.GetAllEvents();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}
