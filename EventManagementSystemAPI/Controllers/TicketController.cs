using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Entities;
using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("tickets")]
    [ValidateModel]
    public class TicketController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly IPurchaseTicketUseCase _purchaseTicketUseCase;
        private readonly IGetUserTicketsUseCase _getUserTicketsUseCase;

        public TicketController(IPurchaseTicketUseCase purchaseTicketUseCase, ICreateUserUseCase createUserUseCase, IGetUserTicketsUseCase getUserTicketsUseCase)
        {
            _purchaseTicketUseCase = purchaseTicketUseCase;
            _createUserUseCase = createUserUseCase;
            _getUserTicketsUseCase = getUserTicketsUseCase;
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

        [Authorize(Policy = ApiPolicies.UserClientRole)]
        [HttpGet("purchased")]
        public async Task<IActionResult> GetUserTickets()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new Response<string>(401, "User not authenticated.", null));
                }

                var tickets = await _getUserTicketsUseCase.Execute(userId);

                if (!tickets.Any())
                {
                    return Ok(new Response<IEnumerable<TicketDto>>(
                        200,
                        "No tickets found for the user.",
                        new List<TicketDto>()
                    ));
                }

                return Ok(new Response<IEnumerable<TicketDto>>(
                    200,
                    "Tickets retrieved successfully.",
                    tickets
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>(
                    500,
                    "An error occurred while retrieving the tickets.",
                    ex.Message
                ));
            }
        }
    }
}
