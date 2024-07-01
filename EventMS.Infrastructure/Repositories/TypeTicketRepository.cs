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
    public class TypeTicketRepository : ITypeTicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TypeTicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TypeTicket> GetAllTypeTickets()
        {
            return _context.TypeTickets.ToList();
        }

        public async Task<TypeTicket> GetTypeTicketById(int id)
        {
            return await _context.TypeTickets.FirstOrDefaultAsync(tt => tt.Id == id);
        }
        public void AddTypeTicket(TypeTicket typeTicket)
        {
            _context.TypeTickets.Add(typeTicket);
            _context.SaveChanges();
        }

        public async Task UpdateTypeTicket(TypeTicket typeTicket)
        {
            _context.TypeTickets.Update(typeTicket);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<TypeTicketCount> GetTicketTypeCounts(int eventId)
        {
            return _context.Tickets
                .Where(t => t.EventId == eventId)
                .GroupBy(t => t.TypeTicket)
                .Select(g => new TypeTicketCount
                {
                    TypeTicket = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }
    }
}
