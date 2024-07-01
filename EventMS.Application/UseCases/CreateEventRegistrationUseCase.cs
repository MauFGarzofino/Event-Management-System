using AutoMapper;
using EventMS.Application.DTOs;
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
    public class CreateEventRegistrationUseCase : ICreateEventRegistrationUseCase
    {
        private readonly IEventRegistrationRepository _eventRegistrationRepository;
        private readonly IMapper _mapper;

        public CreateEventRegistrationUseCase(IEventRegistrationRepository eventRegistrationRepository, IMapper mapper)
        {
            _eventRegistrationRepository = eventRegistrationRepository;
            _mapper = mapper;
        }

        public EventRegistration Execute(EventRegistrationDto eventRegistrationDto)
        {
            var eventRegistration = _mapper.Map<EventRegistration>(eventRegistrationDto);
            _eventRegistrationRepository.AddEventRegistration(eventRegistration);
            return eventRegistration;
        }
    }
}
