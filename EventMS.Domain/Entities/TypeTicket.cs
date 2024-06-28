using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class TypeTicket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
