using EventMS.Infrastructure.Data;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Infrastructure.Repositories
{
    public class TicketRepository: ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddTicket(Ticket newTicket)
        {
            _context.Tickets.Add(newTicket);
            _context.SaveChanges();
        }
    }
}
