using EventManagementSystemAPI.Filters;
using EventManagementSystemAPI.Filters.validations;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
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
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;

        public EventController(ICreateEventUseCase createEventUseCase, IUpdateEventUseCase updateEventUseCase)
        {
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
        }

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
                return CreatedAtAction(nameof(Post), new { id = createdEvent.Id }, createdEvent);

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

    }
}
