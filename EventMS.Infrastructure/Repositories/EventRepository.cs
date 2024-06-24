using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.Include(e => e.Tickets).ToList();
        }

        public void AddEvent(Event newEvent)
        {
            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }

        public void UpdateEvent(Event updatedEvent)
        {
            var local = _context.Set<Event>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(updatedEvent.Id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Entry(updatedEvent).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Event GetEventById(int id)
        {
            return _context.Events.Include(e => e.Tickets).FirstOrDefault(e => e.Id == id);
        }

        public bool EventExists(string title, DateTime date, TimeSpan time, string location)
        {
            return _context.Events.Any(e => e.Title == title && e.Date == date && e.Time == time && e.Location == location);
        }
    }
}
