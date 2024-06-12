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

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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
        public ActionResult<Event> Post([FromBody] Event newEvent)
        {
            _eventRepository.AddEvent(newEvent);
            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, newEvent);
        }
    }
}
