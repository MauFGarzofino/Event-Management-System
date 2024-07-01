using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Infrastructure.Auth.TokenManagement;
using EventMS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("event-registrations")]
    public class EventRegistrationController : ControllerBase
    {
        private readonly ICreateEventRegistrationUseCase _createEventRegistrationUseCase;
        private readonly IDeleteEventRegistrationUseCase _deleteEventRegistrationUseCase;

        public EventRegistrationController(
            ICreateEventRegistrationUseCase createEventRegistrationUseCase,
            IDeleteEventRegistrationUseCase deleteEventRegistrationUseCase)
        {
            _createEventRegistrationUseCase = createEventRegistrationUseCase;
            _deleteEventRegistrationUseCase = deleteEventRegistrationUseCase;
        }

        [HttpPost]
        public IActionResult Post([FromBody] EventRegistrationDto eventRegistrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdRegistration = _createEventRegistrationUseCase.Execute(eventRegistrationDto);
                return CreatedAtAction(nameof(Post), new { id = createdRegistration.Id }, new
                {
                    status = 201,
                    message = "Event registration created successfully.",
                    data = createdRegistration
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteEventRegistrationUseCase.Execute(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    message = ex.Message
                });
            }
        }
    }
}
