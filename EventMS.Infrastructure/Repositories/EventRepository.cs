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
            return _context.Events.ToList();
        }

        public void AddEvent(Event newEvent)
        {
            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }
        
        //methods to be implemented
        public Event GetEventById(int id) => throw new NotImplementedException();
        public void UpdateEvent(Event updatedEvent) => throw new NotImplementedException();
        public void DeleteEvent(int id) => throw new NotImplementedException();
    }
}
