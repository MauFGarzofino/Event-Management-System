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

        [HttpPost]
        public ActionResult<Event> Post([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event data is required.");
            }

            if (string.IsNullOrWhiteSpace(newEvent.Title) || newEvent.Date == default || newEvent.Time == default || string.IsNullOrWhiteSpace(newEvent.Location))
            {
                return BadRequest("Missing mandatory fields: title, date, time, location.");
            }

            if (_eventRepository.EventExists(newEvent.Title, newEvent.Date, newEvent.Location))
            {
                return Conflict("An event with the same title, date, and location already exists.");
            }

            _eventRepository.AddEvent(newEvent);
            return CreatedAtAction(nameof(Post), new { id = newEvent.Id }, newEvent);
        }
    }
}
