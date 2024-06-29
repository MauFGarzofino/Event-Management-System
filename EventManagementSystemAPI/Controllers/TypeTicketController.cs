using EventManagementSystemAPI.Filters.validations;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystemAPI.Models;


namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("typetickets")]
    [ValidateModel]
    public class TypeTicketController : ControllerBase
    {
        private readonly ICreateTypeTicketUseCase _createTypeTicketUseCase;
        private readonly IDeleteTicketsByIdUseCase _deleteTicketsUseCase;

        public TypeTicketController(
            ICreateTypeTicketUseCase createTypeTicketUseCase,
            IDeleteTicketsByIdUseCase deleteTicketsUseCase)
        {
            _createTypeTicketUseCase = createTypeTicketUseCase;
            _deleteTicketsUseCase = deleteTicketsUseCase;
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

        [Authorize(Policy = ApiPolicies.OrganizerClientRole)]
        [HttpDelete("{eventId}/tickets")]
        public IActionResult DeleteTicketsByEventId(int eventId)
        {
            try
            {
                _deleteTicketsUseCase.Execute(eventId);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar tickets.", error = ex.Message });
            }
        }

    }

}
