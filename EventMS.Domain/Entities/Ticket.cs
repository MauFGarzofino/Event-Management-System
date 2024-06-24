using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public int QuantityAvailable { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
