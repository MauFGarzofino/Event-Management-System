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

        public TypeTicketController(ICreateTypeTicketUseCase createTypeTicketUseCase)
        {
            _createTypeTicketUseCase = createTypeTicketUseCase;
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
    }

}
