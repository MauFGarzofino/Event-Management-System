using EventManagementSystemAPI.Controllers;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventManagementSystemAPI.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IGetAllEventsUseCase> _mockGetAllEventsUseCase;
        private readonly Mock<ICreateEventUseCase> _mockCreateEventUseCase;
        private readonly Mock<IUpdateEventUseCase> _mockUpdateEventUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockGetAllEventsUseCase = new Mock<IGetAllEventsUseCase>();
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _mockUpdateEventUseCase = new Mock<IUpdateEventUseCase>();
            _controller = new EventController(_mockGetAllEventsUseCase.Object, _mockCreateEventUseCase.Object, _mockUpdateEventUseCase.Object);
        }

        [Fact]
        public void Get_ShouldReturnOkResult_WhenEventsExist()
        {
            // Arrange
            var eventDtos = new List<EventDto>
            {
                new EventDto { Title = "Event 1" },
                new EventDto { Title = "Event 2" }
            };
            _mockGetAllEventsUseCase.Setup(u => u.Execute()).Returns(eventDtos);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<EventDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void Get_ShouldReturnNoContent_WhenNoEventsExist()
        {
            // Arrange
            var eventDtos = new List<EventDto>();
            _mockGetAllEventsUseCase.Setup(u => u.Execute()).Returns(eventDtos);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Post_ShouldReturnCreatedAtAction_WhenEventIsCreated()
        {
            // Arrange
            var newEventDto = new EventDto
            {
                Title = "New Event",
                Date = DateTime.Now,
                Time = TimeSpan.FromHours(2),
                Location = "Test Location"
            };

            var createdEvent = new Event
            {
                Id = 1,
                Title = "New Event",
                Date = newEventDto.Date,
                Time = newEventDto.Time,
                Location = newEventDto.Location
            };

            _mockCreateEventUseCase.Setup(x => x.Execute(It.IsAny<EventDto>())).Returns(createdEvent);

            // Act
            var result = _controller.Post(newEventDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Post", createdAtActionResult.ActionName);
            Assert.Equal(createdEvent.Id, ((Event)createdAtActionResult.Value).Id);
        }

        [Fact]
        public void Post_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Post(new EventDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}