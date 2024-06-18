using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
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
        private readonly Mock<ICreateEventUseCase> _mockCreateEventUseCase;
        private readonly Mock<IUpdateEventUseCase> _mockUpdateEventUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _mockUpdateEventUseCase = new Mock<IUpdateEventUseCase>();
            _controller = new EventController(_mockCreateEventUseCase.Object, _mockUpdateEventUseCase.Object);
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

            
            Assert.Equal("Post", createdAtActionResult.ActionName);
            Assert.Equal(201, ((Response<Event>)createdAtActionResult.Value).Status);
            Assert.Equal("Event created successfully.", ((Response<Event>)createdAtActionResult.Value).Message);
            Assert.Null(((Response<Event>)createdAtActionResult.Value).Errors);
            Assert.Equal(createdEvent.Id, ((Response<Event>)createdAtActionResult.Value).Data.Id);

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