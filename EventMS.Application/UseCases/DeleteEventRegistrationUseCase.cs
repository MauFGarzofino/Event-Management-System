using EventMS.Application.Ports;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class DeleteEventRegistrationUseCase : IDeleteEventRegistrationUseCase
    {
        private readonly IEventRegistrationRepository _eventRegistrationRepository;

        public DeleteEventRegistrationUseCase(IEventRegistrationRepository eventRegistrationRepository)
        {
            _eventRegistrationRepository = eventRegistrationRepository;
        }

        public void Execute(int registrationId)
        {
            _eventRegistrationRepository.DeleteEventRegistration(registrationId);
        }
    }
}
