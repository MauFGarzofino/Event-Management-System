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
        IEnumerable<Ticket> GetAllTickets();
        void AddTicket(Ticket newTicket);
        //void UpdateTicket(Ticket updatedTicket);
        Ticket GetTicketById(int id);
        bool TicketExists(int id);
    }
}
