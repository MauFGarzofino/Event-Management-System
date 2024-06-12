using EventManagementSystemAPI.Models;
using EventMS.Application.Port;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ICreateEventUseCase _createEventUseCase;

        public EventController(ICreateEventUseCase createEventUseCase)
        {
            _createEventUseCase = createEventUseCase;
        }

        [HttpPost]
        public IActionResult Post([FromBody] EventDto newEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEvent = new Event
            {
                Title = newEventDto.Title,
                Description = newEventDto.Description,
                Date = newEventDto.Date,
                Time = newEventDto.Time,
                Location = newEventDto.Location
            };

            try
            {
                _createEventUseCase.Execute(newEvent);
                return CreatedAtAction(nameof(Post), new { id = newEvent.Id }, newEvent);
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
