using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("tickets")]
    [ValidateModel]
    public class TicketController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IPurchaseTicketUseCase _purchaseTicketUseCase;

        public TicketController(IPurchaseTicketUseCase purchaseTicketUseCase, ICreateUserUseCase createUserUseCase)
        {
            _purchaseTicketUseCase = purchaseTicketUseCase;
            _createUserUseCase = createUserUseCase;
        }

        [Authorize(Policy = ApiPolicies.UserClientRole)]
        [HttpPost("events/{eventId}/tickets-type/{ticketTypeId}")]
        public async Task<IActionResult> PurchaseATicket(int ticketTypeId, int eventId)
        {

            try
            {
                               
                var ticket = await _purchaseTicketUseCase.Execute(ticketTypeId, User, eventId);

                if(ticket == null)
                {
                    return BadRequest(new Response<string>(400, "Insufficient tickets available.", null));
                }

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
                    ex.Message,
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
