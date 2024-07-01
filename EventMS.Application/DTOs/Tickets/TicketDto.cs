using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.DTOs.Tickets
{
    public class TicketDto
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string UserId { get; set; }
        public int TypeTicketId { get; set; }
        public string TypeTicketName { get; set; }
    }
}
