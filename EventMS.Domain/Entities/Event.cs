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

        public Event(string title, string description, DateTime date, TimeSpan time, string location)
        {
            _title = title;
            _description = description;
            _date = date;
            _time = time;
            _location = location;
            Tickets = new List<Ticket>();
            TypeTickets = new List<TypeTicket>();
            EventRegistrations = new List<EventRegistration>();
        }

        private Event()
        {
            Tickets = new List<Ticket>();
            TypeTickets = new List<TypeTicket>();
            EventRegistrations = new List<EventRegistration>();
        }

        public int Id { get; set; }
        public string Title => _title;
        public string Description => _description;
        public DateTime Date => _date;
        public TimeSpan Time { get; private set; }
        public string Location => _location;
        [JsonIgnore]
        public ICollection<Ticket> Tickets { get; private set; }
        [JsonIgnore]
        public ICollection<TypeTicket> TypeTickets { get; private set; }
        [JsonIgnore]
        public ICollection<EventRegistration> EventRegistrations { get; private set; }

        public void AddTypeTicket(TypeTicket typeTicket)
        {
            TypeTickets.Add(typeTicket);
        }
    }
}