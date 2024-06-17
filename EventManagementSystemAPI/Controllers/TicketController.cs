using EventMS.Application.DTOs;
using EventMS.Application.Port;
using Microsoft.AspNetCore.Mvc;
using EventMS.Application.Ports;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("events/{eventId}/tickets")]
    public class TicketController: ControllerBase
    {
        private readonly ICreateTicketUseCase _createTicketUseCase;

        public TicketController(ICreateTicketUseCase createTicketUseCase)
        {
            _createTicketUseCase = createTicketUseCase;
        }

        [HttpPost]
        public IActionResult Post(int eventId, [FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ticketDto.EventId = eventId;

            try
            {
                var createdTicket = _createTicketUseCase.Execute(ticketDto);
                return CreatedAtAction(nameof(Post), new { id = createdTicket.Id }, createdTicket);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }


        }
    }
}
