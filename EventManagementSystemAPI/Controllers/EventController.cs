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
using System;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventController : ControllerBase
    {
        private readonly IGetAllEventsUseCase _getAllEventsUseCase;
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;

        public EventController(IGetAllEventsUseCase getAllEventsUseCase, ICreateEventUseCase createEventUseCase, IUpdateEventUseCase updateEventUseCase, IDeleteEventUseCase deleteEventUseCase)
        {
            _getAllEventsUseCase = getAllEventsUseCase;
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
        }

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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateEventDto updatedEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            updatedEventDto.Id = id;

            try
            {
                var updatedEvent = _updateEventUseCase.Execute(updatedEventDto);
                return Ok(updatedEvent);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(string title)
        {
            try
            {
                var deleteDto = new DeleteEventDto { Title = title };
                _deleteEventUseCase.Execute(deleteDto);
                return Ok("Event deleted successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event was not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
