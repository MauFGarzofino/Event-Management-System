using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventMS.Domain.Entities;

namespace EventMS.Application.Ports
{
    public interface IPurchaseTicketUseCase
    {
        Task<Domain.Entities.Ticket> Execute(int ticketTypeId, ClaimsPrincipal user, int eventId);
    }
}
