using EventMS.Application.DTOs;
using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface ICreateTicketUseCase
    {
        Ticket Execute(TicketDto ticketDto);
    }
}
