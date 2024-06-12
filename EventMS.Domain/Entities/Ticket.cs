using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    internal class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime SaleStartDateTime { get; set; }
        public DateTime SaleEndDateTime { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
