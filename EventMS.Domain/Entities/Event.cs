using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class Event
    {
        private DateTime _date;
        private TimeSpan _time;
        private string _title;
        private string _description;
        private string _location;
        private List<Ticket> _tickets;

        public Event(string title, string description, DateTime date, TimeSpan time, string location)
        {
            _title = title;
            _description = description;
            _date = date;
            _time = time;
            _location = location;
            _tickets = new List<Ticket>();
        }

        // Constructor sin parámetros requerido por EF
        private Event()
        {
            _tickets = new List<Ticket>();
        }

        public int Id { get; set; }
        public string Title => _title;
        public string Description => _description;
        public DateTime Date => _date;
        public TimeSpan Time { get; private set; }
        public string Location => _location;
        public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

        public void UpdateDetails(string title, string description, DateTime date, TimeSpan time, string location)
        {
            _title = title;
            _description = description;
            _date = date;
            _time = time;
            _location = location;
        }

        public void AddTicket(Ticket ticket)
        {
            if (_tickets.Count >= 100)
            {
                throw new InvalidOperationException("Cannot add more tickets, event is full.");
            }
            _tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            _tickets.Remove(ticket);
        }
    }

}