using EventManagementSystemAPI.Filters;
using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Auth.TokenManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("events")]
    [ValidateModel]
    public class EventController : ControllerBase
    {
        private readonly IGetAllEventsUseCase _getAllEventsUseCase;
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;
        private readonly IGetEventByIdUseCase _getEventByIdUseCase;

        public EventController(
            IGetAllEventsUseCase getAllEventsUseCase,
            ICreateEventUseCase createEventUseCase,
            IUpdateEventUseCase updateEventUseCase,
            IDeleteEventUseCase deleteEventUseCase,
            IGetEventByIdUseCase getEventByIdUseCase)
        {
            _getAllEventsUseCase = getAllEventsUseCase;
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _getEventByIdUseCase = getEventByIdUseCase;
        }

        [Authorize(Policy = ApiPolicies.UserClientRole)]
        [HttpGet]
        public IActionResult Get()
        {
            var events = _getAllEventsUseCase.Execute();

            if (!events.Any())
            {
                return NoContent();
            }

            return Ok(events);
        }

        [Authorize(Policy = ApiPolicies.OrganizerClientRole)]
        [HttpPost]
        public IActionResult Post([FromBody] EventDto newEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdEvent = _createEventUseCase.Execute(newEventDto);

                return CreatedAtAction(nameof(Post), new { id = createdEvent.Id }, new Response<Event>(
                    201,
                    "Event created successfully.",
                    createdEvent
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
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateEventDto updatedEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<Dictionary<string, string[]>>(
                    400,
                    "Validation failed. Please check the provided data.",
                    ModelState.ToDictionary(
                        m => m.Key,
                        m => m.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                ));
            }

            updatedEventDto.Id = id;

            try
            {
                var updatedEvent = _updateEventUseCase.Execute(updatedEventDto);
                return Ok(new Response<Event>(
                    200,
                    "Event updated successfully.",
                    updatedEvent
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _deleteEventUseCase.Execute(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string>(
                    404,
                    ex.Message,
                    null
                ));
            }
        }

        [Authorize(Policy = ApiPolicies.OrganizerClientRole)]
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            try
            {
                var eventDto = _getEventByIdUseCase.Execute(id);
                if (eventDto == null)
                {
                    return NotFound(new Response<string>(
                        404,
                        "Event not found",
                        null
                    ));
                }

                return Ok(new Response<EventDto>(
                    200,
                    "Event details retrieved successfully",
                    eventDto
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>(
                    500,
                    ex.Message,
                    null
                ));
            }
        }
    }
}
