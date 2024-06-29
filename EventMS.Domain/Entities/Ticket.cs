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
        public string UserId { get; private set; }
        public User User { get; private set; }
        public int TypeTicketId { get; private set; }
        public TypeTicket TypeTicket { get; private set; }

        private Ticket() { }

        public Ticket(User user, TypeTicket typeTicket)
        {
            PurchaseDate = DateTime.Now;
            User = user ?? throw new ArgumentNullException(nameof(user));
            TypeTicket = typeTicket ?? throw new ArgumentNullException(nameof(typeTicket));
            UserId = user.Id;
            TypeTicketId = typeTicket.Id;
        }
    }
}
