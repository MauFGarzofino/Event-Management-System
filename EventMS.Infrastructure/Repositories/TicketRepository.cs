using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.Include(t => t.Event).Include(t => t.User).ToListAsync();
        }

        public async Task AddTicket(Ticket newTicket)
        {
            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket> GetTicketById(int id)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> TicketExists(int id)
        {
            return await _context.Tickets.AnyAsync(t => t.Id == id);
        }
    }
}
