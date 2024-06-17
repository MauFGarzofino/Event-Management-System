using EventMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface IGetAllEventsUseCase
    {
        IEnumerable<EventDto> Execute();
    }
}
