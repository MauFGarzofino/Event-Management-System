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
        public int Id { get; set; }
        public DateTime PurchaseDate { get; private set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; private set; }
        public int TypeTicketId { get; set; }
        [JsonIgnore]
        public TypeTicket TypeTicket { get; private set; }

        public Ticket() { }

        public Ticket(string userId, int typeTicketId)
        {
            UserId = userId;
            TypeTicketId = typeTicketId;
            PurchaseDate = DateTime.Now;
        }
    }
}
