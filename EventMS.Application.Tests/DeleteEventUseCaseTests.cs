using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Moq;
using Xunit;

namespace EventMS.Application.Tests
{
    public class DeleteEventUseCaseTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly DeleteEventUseCase _deleteEventUseCase;

        public DeleteEventUseCaseTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _deleteEventUseCase = new DeleteEventUseCase(_mockEventRepository.Object);
        }

        [Fact]
        public void DeleteEvent_EventExists_DeletesEvent()
        {
            // Arrange
            int eventId = 1;
            var existingEvent = new Event { Id = eventId };
            _mockEventRepository.Setup(x => x.GetEventById(eventId)).Returns(existingEvent);

            // Act
            _deleteEventUseCase.DeleteEvent(eventId);

            // Assert
            _mockEventRepository.Verify(x => x.DeleteEvent(existingEvent), Times.Once);
        }

        [Fact]
        public void DeleteEvent_EventNotFound_ThrowsArgumentException()
        {
            // Arrange
            int eventId = 1;
            _mockEventRepository.Setup(x => x.GetEventById(eventId)).Returns((Event)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _deleteEventUseCase.DeleteEvent(eventId));
        }
    }
}
