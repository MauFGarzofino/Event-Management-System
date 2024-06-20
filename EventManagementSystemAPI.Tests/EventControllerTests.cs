using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EventManagementSystemAPI.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IGetAllEventsUseCase> _mockGetAllEventsUseCase;
        private readonly Mock<ICreateEventUseCase> _mockCreateEventUseCase;
        private readonly Mock<IUpdateEventUseCase> _mockUpdateEventUseCase;
        private readonly Mock<IDeleteEventUseCase> _mockDeleteEventUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockGetAllEventsUseCase = new Mock<IGetAllEventsUseCase>();
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _mockUpdateEventUseCase = new Mock<IUpdateEventUseCase>();
            _mockDeleteEventUseCase = new Mock<IDeleteEventUseCase>();
            _controller = new EventController(
                _mockGetAllEventsUseCase.Object,
                _mockCreateEventUseCase.Object,
                _mockUpdateEventUseCase.Object,
                _mockDeleteEventUseCase.Object
                );
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

        [Fact]
        public void Delete_EventExists_ReturnsOk()
        {
            // Arrange
            int eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.DeleteEvent(eventId));

            // Act
            var result = _controller.Delete(eventId); // Cambio aquí a _controller

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Event deleted successfully", okResult.Value);
        }

        [Fact]
        public void Delete_EventNotFound_ReturnsNotFound()
        {
            // Arrange
            int eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.DeleteEvent(eventId)).Throws(new ArgumentException("Event not found"));

            // Act
            var result = _controller.Delete(eventId); // Cambio aquí a _controller

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Event not found", notFoundResult.Value);
        }

        [Fact]
        public void Delete_InternalServerError_ReturnsStatusCode500()
        {
            // Arrange
            int eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.DeleteEvent(eventId)).Throws(new Exception("Some internal error"));

            // Act
            var result = _controller.Delete(eventId); // Cambio aquí a _controller

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}