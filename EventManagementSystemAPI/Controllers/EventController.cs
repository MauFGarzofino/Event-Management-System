using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

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
        public IEnumerable<Event> Get()
        {
            return _eventRepository.GetAllEvents();
        }

        [HttpGet("{id}")]
        public ActionResult<Event> Get(int id)
        {
            var eventItem = _eventRepository.GetEventById(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return eventItem;
        }

        [HttpPost]
        public ActionResult<Event> Post([FromBody] Event newEvent)
        {
            _eventRepository.AddEvent(newEvent);
            return CreatedAtAction(nameof(Get), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }

            var eventToUpdate = _eventRepository.GetEventById(id);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            _eventRepository.UpdateEvent(updatedEvent);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var eventToDelete = _eventRepository.GetEventById(id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            _eventRepository.DeleteEvent(id);
            return NoContent();
        }

        [HttpPost("{eventId}/tickets")]
        public IActionResult CreateTicket(int eventId, [FromBody] Ticket newTicket)
        {
            if (newTicket == null)
            {
                return BadRequest("Ticket data is null.");
            }

            if (eventId != newTicket.EventId)
            {
                return BadRequest("Event ID mismatch.");
            }

            try
            {
                _eventRepository.AddTicket(newTicket);
                return CreatedAtAction(nameof(GetTicketById), new { eventId = eventId, id = newTicket.Id }, newTicket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{eventId}/tickets/{id}")]
        public IActionResult GetTicketById(int eventId, int id)
        {
            var ticket = _eventRepository.GetTicketById(eventId, id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
    }

}
}
