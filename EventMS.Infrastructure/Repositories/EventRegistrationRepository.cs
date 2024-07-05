using EventMS.Domain.Entities;
using EventMS.Infrastructure.Data;
using EventMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Infrastructure.Repositories
{
    public class EventRegistrationRepository : IEventRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddEventRegistration(EventRegistration eventRegistration)
        {
            _context.EventRegistrations.Add(eventRegistration);
            _context.SaveChanges();
        }

 
        public void DeleteEventRegistration(string userId, int registrationId)
        {
            var registration = _context.EventRegistrations
                .FirstOrDefault(er => er.UserId == userId && er.Id == registrationId);
            if (registration != null)
            {
                _context.EventRegistrations.Remove(registration);
                _context.SaveChanges();
            }
        }
        public IEnumerable<EventRegistration> GetByUserId(string userId)
        {
            return _context.EventRegistrations.Where(er => er.UserId == userId).ToList();
        }
    }
}
