﻿using EventMS.Domain.Entities;
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
        void AddEvent(Event newEvent);
        void UpdateEvent(Event newEvent);
        void DeleteEvent(int id);
        bool EventExists(string title, DateTime date, TimeSpan time, string location);
        Event GetEventById(int id);
    }
}
