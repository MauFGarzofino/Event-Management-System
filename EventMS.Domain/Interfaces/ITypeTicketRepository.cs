using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Interfaces
{
    public interface ITypeTicketRepository
    {
        IEnumerable<TypeTicket> GetAllTypeTickets();
        TypeTicket GetTypeTicketById(int id);
        void AddTypeTicket(TypeTicket typeTicket);
        void UpdateTypeTicket(TypeTicket typeTicket);
        IEnumerable<TypeTicketCount> GetTicketTypeCounts(int eventId);
    }
}
