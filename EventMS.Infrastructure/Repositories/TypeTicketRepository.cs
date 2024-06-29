using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Data;
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

        public TypeTicket GetTypeTicketById(int id)
        {
            return _context.TypeTickets.FirstOrDefault(tt => tt.Id == id);
        }
        public void AddTypeTicket(TypeTicket typeTicket)
        {
            _context.TypeTickets.Add(typeTicket);
            _context.SaveChanges();
        }

        public void UpdateTypeTicket(TypeTicket typeTicket)
        {
            _context.TypeTickets.Update(typeTicket);
            _context.SaveChanges();
        }

        public void DeleteTicketsByEventId(int eventId)
        {
            var ticketsToDelete = _context.TypeTickets
                .Where(tt => tt.Tickets.Any(t => t.EventId == eventId))
                .ToList();

            foreach (var typeTicket in ticketsToDelete)
            {
                _context.TypeTickets.Remove(typeTicket);
            }

            _context.SaveChanges();
        }
    }
}
