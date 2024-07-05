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
        private readonly IGetEventRegistrationsByUserUseCase _getEventRegistrationsByUserUseCase;

        public EventRegistrationController(
            ICreateEventRegistrationUseCase createEventRegistrationUseCase,
            IDeleteEventRegistrationUseCase deleteEventRegistrationUseCase,
            IGetEventRegistrationsByUserUseCase getEventRegistrationsByUserUseCase)
        {
            _createEventRegistrationUseCase = createEventRegistrationUseCase;
            _deleteEventRegistrationUseCase = deleteEventRegistrationUseCase;
            _getEventRegistrationsByUserUseCase = getEventRegistrationsByUserUseCase;
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

        [HttpDelete("{userId}/{registrationId}")]
        public IActionResult Delete(string userId, int registrationId)
        {
            try
            {
                _deleteEventRegistrationUseCase.Execute(userId, registrationId);
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

        [HttpGet("user/{userId}")]
        public IActionResult GetByUserId(string userId)
        {
            try
            {
                var registrations = _getEventRegistrationsByUserUseCase.Execute(userId);
                return Ok(new
                {
                    status = 200,
                    message = "Event registrations retrieved successfully.",
                    data = registrations
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
    }
}
