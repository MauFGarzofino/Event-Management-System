using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
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
        private readonly Mock<IGetEventByIdUseCase> _mockGetEventByIdUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockGetAllEventsUseCase = new Mock<IGetAllEventsUseCase>();
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _mockUpdateEventUseCase = new Mock<IUpdateEventUseCase>();
            _mockDeleteEventUseCase = new Mock<IDeleteEventUseCase>();
            _mockGetEventByIdUseCase = new Mock<IGetEventByIdUseCase>();
            _controller = new EventController(
                _mockGetAllEventsUseCase.Object,
                _mockCreateEventUseCase.Object,
                _mockUpdateEventUseCase.Object,
                _mockDeleteEventUseCase.Object,
                _mockGetEventByIdUseCase.Object
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

            var createdEvent = new Event("New Event", "Description",newEventDto.Date, newEventDto.Time, newEventDto.Location)
            {
                Id = 1
            };

            _mockCreateEventUseCase.Setup(x => x.Execute(It.IsAny<EventDto>())).Returns(createdEvent);

            // Act
            var result = _controller.Post(newEventDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
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
        public void Delete_ShouldReturnNoContent_WhenEventIsDeleted()
        {
            // Arrange
            var eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.Execute(eventId));

            // Act
            var result = _controller.Delete(eventId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.Execute(eventId)).Throws(new KeyNotFoundException());

            // Act
            var result = _controller.Delete(eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetEventById_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            int eventId = 1;
            _mockGetEventByIdUseCase.Setup(u => u.Execute(eventId)).Throws(new KeyNotFoundException());

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

    }
}