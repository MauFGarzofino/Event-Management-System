using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventMS.Application.DTOs;
using EventMS.Domain.Entities;

namespace EventMS.Application.DTOs
{
    public class TypeTicketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public ICollection<TypeTicket> Tickets { get; set; }
    }
}
