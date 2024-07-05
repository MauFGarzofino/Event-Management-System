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
        private readonly ITicketRepository _ticketRepository;

        public TypeTicketRepository(ApplicationDbContext context, ITicketRepository ticketRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;
        }

        public IEnumerable<TypeTicket> GetAllTypeTickets()
        {
            return _context.TypeTickets.ToList();
        }

        public async Task<TypeTicket> GetTypeTicketById(int id)
        {
            return await _context.TypeTickets.FirstOrDefaultAsync(tt => tt.Id == id);
        }

        public async Task AddTypeTicketAsync(TypeTicket typeTicket)
        {
            _context.TypeTickets.Add(typeTicket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTypeTicketAsync(TypeTicket typeTicket)
        {
            _context.TypeTickets.Update(typeTicket);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TypeTicketCount>> GetTicketTypeCountsAsync(int eventId)
        {
            return await _context.TypeTickets
                .Where(tt => tt.EventId == eventId)
                .Select(tt => new TypeTicketCount
                {
                    TypeName = tt.Name,
                    Description = tt.Description,
                    Price = tt.Price,
                    QuantityAvailable = tt.QuantityAvailable
                }).ToListAsync();
        }

        public async Task DelteAllTypeTicketsFroAnEvent(int eventId) 
        {

            var typeTickets = await _context.TypeTickets
                                            .Where(tt => tt.EventId == eventId)
                                            .ToListAsync();

            foreach (var typeTicket in typeTickets)
            {
                await _ticketRepository.DeleteTicketsForTypeTickets(typeTicket.Id);
            }

            await _context.SaveChangesAsync();

        }
    }
}
