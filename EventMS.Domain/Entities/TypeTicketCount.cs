using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class TypeTicketCount
    {
        public TypeTicket TypeTicket { get; set; }
        public int Count { get; set; }
    }
}
