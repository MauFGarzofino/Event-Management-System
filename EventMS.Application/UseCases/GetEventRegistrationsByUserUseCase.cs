using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    // Application/UseCases/GetEventRegistrationsByUserUseCase.cs
    public class GetEventRegistrationsByUserUseCase : IGetEventRegistrationsByUserUseCase
    {
        private readonly IEventRegistrationRepository _repository;
        private readonly IMapper _mapper;

        public GetEventRegistrationsByUserUseCase(IEventRegistrationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<EventRegistrationDto> Execute(string userId)
        {
            var registrations = _repository.GetByUserId(userId);
            return _mapper.Map<IEnumerable<EventRegistrationDto>>(registrations);
        }
    }

}
