using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class Ticket
    {
        // Propiedades
        public int Id { get; set; }
        public DateTime PurchaseDate { get; private set; }
        public int EventId { get;  set; }
        [JsonIgnore]
        public Event Event { get; private set; }
        public string UserId { get; private set; }
        [JsonIgnore]
        public User User { get; private set; }
        public int TypeTicketId { get; private set; }
        [JsonIgnore]
        public TypeTicket TypeTicket { get; private set; }

        public Ticket() { }
        public Ticket(int eventId, string userId, int typeTicketId) 
        {
            EventId = eventId;
            UserId = userId;
            TypeTicketId = typeTicketId;
            PurchaseDate = DateTime.Now;
        }


        public Ticket(Event ev, User user, TypeTicket typeTicket)
        {
            PurchaseDate = DateTime.Now;
            Event = ev ?? throw new ArgumentNullException(nameof(ev));
            User = user ?? throw new ArgumentNullException(nameof(user));
            TypeTicket = typeTicket ?? throw new ArgumentNullException(nameof(typeTicket));
            EventId = ev.Id;
            UserId = user.Id;
            TypeTicketId = typeTicket.Id;
        }
    }
}
