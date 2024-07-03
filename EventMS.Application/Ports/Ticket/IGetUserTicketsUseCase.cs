using EventMS.Application.DTOs.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports.Ticket
{
    public interface IGetUserTicketsUseCase
    {
        Task<IEnumerable<TicketDto>> Execute(string userId);
    }
}
