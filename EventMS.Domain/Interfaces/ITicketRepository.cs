using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTickets();
        Task AddTicket(Ticket newTicket);
        Task<Ticket> GetTicketById(int id);
        Task<bool> TicketExists(int id);
    }

}
