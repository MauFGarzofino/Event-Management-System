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

        public Event GetEventByTitle(string title)
        {
            return _context.Events.FirstOrDefault(e => e.Title == title);
        }
        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events.ToList();
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

            // check if local is not null 
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Entry(updatedEvent).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEvent(int eventId)
        {
            var eventToDelete = _context.Events.Find(eventId);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Event with id {eventId} not found.");
            }
        }

        public Event GetEventById(int id)
        {
            return _context.Events.Find(id);
        }

        public bool EventExists(string title, DateTime date, string location)
        {
            return _context.Events.Any(e => e.Title == title && e.Date == date && e.Location == location);
        }
    }
}
