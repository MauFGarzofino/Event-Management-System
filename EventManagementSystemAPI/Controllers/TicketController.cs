using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("tickets")]
    [ValidateModel]
    public class TicketController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IPurchaseTicketUseCase _purchaseTicketUseCase;

        public TicketController(IPurchaseTicketUseCase purchaseTicketUseCase)
        {
            _purchaseTicketUseCase = purchaseTicketUseCase;
        }

        [HttpPost("events/tickets-type{ticketTypeId}")]
        public async Task<IActionResult> PurchaseATicket(int ticketTypeId)
        {

            try
            {

                var user = _createUserUseCase.Execute(User);

                var ticket = _purchaseTicketUseCase.Execute(ticketTypeId, user);

                return CreatedAtAction(nameof(PurchaseATicket), new { id = ticket.Id }, new Response<Ticket>(
                    201,
                    "Ticket purchased succesfully.",
                    ticket
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string>(
                    404,
                    "There are not tickets availbale",
                    null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new Response<string>(
                    400,
                    ex.Message,
                    null
                ));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new Response<string>(
                    409,
                    ex.Message,
                    null
                ));
            }

        }

    }
}
