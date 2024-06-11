using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Interfaces
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(int id);
        void AddEvent(Event newEvent);
        void UpdateEvent(Event updatedEvent);
        void DeleteEvent(int id);
        bool EventExists(string title, DateTime date, string location);
    }
}
