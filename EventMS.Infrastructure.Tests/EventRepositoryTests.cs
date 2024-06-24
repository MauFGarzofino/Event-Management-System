using EventMS.Domain.Entities;
using EventMS.Infrastructure.Data;
using EventMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMS.Infrastructure.Tests
{
    public class EventRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EventRepository _eventRepository;

        public EventRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _eventRepository = new EventRepository(_context);
        }

        [Fact]
        public void Should_Add_Event_Successfully()
        {
            // Arrange
            var newEvent = new Event(
                "Test Event",
                "Test Description",
                DateTime.Now,
                TimeSpan.FromHours(14),
                "Test Location"
            );

            // Act
            _eventRepository.AddEvent(newEvent);
            var addedEvent = _context.Events.Find(newEvent.Id);

            // Assert
            Assert.NotNull(addedEvent);
            Assert.Equal(newEvent.Title, addedEvent.Title);
        }

        [Fact]
        public void Should_Return_True_If_Event_Exists()
        {
            // Arrange
            var newEvent = new Event(
                "Existing Event",
                "Test Description",
                DateTime.Now,
                TimeSpan.FromHours(14),
                "Test Location"
            );
            _context.Events.Add(newEvent);
            _context.SaveChanges();

            // Act
            var exists = _eventRepository.EventExists(newEvent.Title, newEvent.Date, newEvent.Time, newEvent.Location);

            // Assert
            Assert.True(exists);
        }
    }
}