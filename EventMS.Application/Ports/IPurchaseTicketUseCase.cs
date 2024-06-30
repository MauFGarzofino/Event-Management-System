using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface IPurchaseTicketUseCase
    {
        Ticket Execute(int ticketId, User userId);
    }
}
