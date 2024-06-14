using EventMS.Domain.Entities;

namespace EventMS.Domain.Tests
{
    public class EventTests
    {
        [Fact]
        public void CanCreateEvent_WithValidDetails()
        {
            var newEvent = new Event
            {
                Title = "New Event",
                Date = new DateTime(2024, 8, 15),
                Time = new TimeSpan(11, 0, 0),
                Location = "City Library"
            };

            Assert.NotNull(newEvent);
            Assert.Equal("New Event", newEvent.Title);
            Assert.Equal(new DateTime(2024, 8, 15), newEvent.Date);
            Assert.Equal(new TimeSpan(11, 0, 0), newEvent.Time);
            Assert.Equal("City Library", newEvent.Location);
        }
    }
}