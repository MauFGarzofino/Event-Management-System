using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICreateEventUseCase _createEventUseCase;

        public EventController(IEventRepository eventRepository, ICreateEventUseCase createEventUseCase)
        {
            _eventRepository = eventRepository;
            _createEventUseCase = createEventUseCase;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var events = _eventRepository.GetAllEvents();
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
    }
}
