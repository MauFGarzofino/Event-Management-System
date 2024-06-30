using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class Ticket
    {
        // Propiedades
        public int Id { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public int EventId { get; private set; }
        public Event Event { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public int TypeTicketId { get; private set; }
        public TypeTicket TypeTicket { get; private set; }

        private Ticket() { }

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
