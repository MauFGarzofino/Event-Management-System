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
        Task<TypeTicket> GetTypeTicketById(int id);
        Task AddTypeTicketAsync(TypeTicket typeTicket);
        Task UpdateTypeTicketAsync(TypeTicket typeTicket);
        Task<IEnumerable<TypeTicketCount>> GetTicketTypeCountsAsync(int eventId);
        Task DelteAllTypeTicketsFroAnEvent(int eventId);

    }
}
