using EventManagementSystemAPI.Filters.validations;
using EventMS.Application.DTOs.Tickets;
using EventMS.Domain.Entities;
using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystemAPI.Models;
using EventMS.Application.Ports.Ticket;


namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("typetickets")]
    [ValidateModel]
    public class TypeTicketController : ControllerBase
    {
        private readonly ICreateTypeTicketUseCase _createTypeTicketUseCase;
        private readonly IGetTicketTypeCountsUseCase _getTicketTypeCountsUseCase;

        public TypeTicketController
            (ICreateTypeTicketUseCase createTypeTicketUseCase,
            IGetTicketTypeCountsUseCase getTicketTypeCountsUseCase)
        {
            _createTypeTicketUseCase = createTypeTicketUseCase;
            _getTicketTypeCountsUseCase = getTicketTypeCountsUseCase;
        }

        [Authorize(Policy = ApiPolicies.OrganizerClientRole)]
        [HttpPost]
        public IActionResult Post([FromBody] TypeTicketDto newTypeTicketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdTypeTicket = _createTypeTicketUseCase.Execute(newTypeTicketDto);

                return CreatedAtAction(nameof(Post), new { id = createdTypeTicket.Id }, new Response<TypeTicket>(
                    201,
                    "Type ticket created successfully.",
                    createdTypeTicket
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

        [Authorize(Policy = "OrganizerClientRole")]
        [HttpGet("{eventId}/ticket-types/count")]
        public IActionResult GetTicketTypeCounts(int eventId)
        {
            var ticketTypeCounts = _getTicketTypeCountsUseCase.Execute(eventId);

            if (!ticketTypeCounts.Any())
            {
                return NotFound(new Response<string>(
                    404,
                    "No ticket types found for the specified event.",
                    null
                ));
            }

            return Ok(new Response<IEnumerable<TicketTypeCountDto>>(
                200,
                "Ticket type counts retrieved successfully.",
                ticketTypeCounts
            ));
        }
    }
}
