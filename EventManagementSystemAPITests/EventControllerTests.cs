using EventManagementSystemAPI.Controllers;
using EventMS.Application.Port;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EventManagementSystemAPITests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly Mock<ICreateEventUseCase> _mockCreateEventUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _controller = new EventController(_mockRepository.Object, _mockCreateEventUseCase.Object);
        }

        [Fact]
        [Trait("Category", "GET")]
        public void Get_ReturnsOkResult_WhenEventsExist()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, Title = "Event 1", Date = DateTime.Now.Date, Time = new TimeSpan(14, 0, 0), Location = "Location 1" },
                new Event { Id = 2, Title = "Event 2", Date = DateTime.Now.AddDays(1).Date, Time = new TimeSpan(15, 0, 0), Location = "Location 2" }
            };
            _mockRepository.Setup(repo => repo.GetAllEvents()).Returns(events);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvents = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);
            Assert.Equal(2, returnedEvents.Count());
        }

        [Fact]
        [Trait("Category", "GET")]
        public void Get_ReturnsNoContent_WhenNoEventsExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllEvents()).Returns(new List<Event>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}