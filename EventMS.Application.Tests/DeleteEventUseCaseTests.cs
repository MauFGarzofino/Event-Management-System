using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Routing;
using Moq;


namespace EventMS.Application.Tests
{
    public class DeleteEventUseCaseTests
    {
        [Fact]
        public void DeleteEvent_Successfully()
        {
            // Arrange
            var mockEventRepository = new Mock<IEventRepository>();
            var deleteEventUseCase = new DeleteEventUseCase(mockEventRepository.Object);
            int eventIdToDelete = 1;

            // Act
            deleteEventUseCase.Execute(eventIdToDelete);

            // Assert
            mockEventRepository.Verify(r => r.DeleteEvent(eventIdToDelete), Times.Once);
        }
    }
}
