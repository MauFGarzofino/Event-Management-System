using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Interfaces
{
    public interface IEventRegistrationRepository
    {
        void AddEventRegistration(EventRegistration eventRegistration);
        void DeleteEventRegistration(int registrationId);
    }
}
