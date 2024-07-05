using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly Mock<IGetEventByTitleUseCase> _mockGetEventByTitleUseCase;
        private readonly Mock<IGetEventsByDateUseCase> _mockGetEventsByDateUseCase;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockGetAllEventsUseCase = new Mock<IGetAllEventsUseCase>();
            _mockCreateEventUseCase = new Mock<ICreateEventUseCase>();
            _mockUpdateEventUseCase = new Mock<IUpdateEventUseCase>();
            _mockDeleteEventUseCase = new Mock<IDeleteEventUseCase>();
            _mockGetEventByIdUseCase = new Mock<IGetEventByIdUseCase>();
            _mockGetEventByTitleUseCase = new Mock<IGetEventByTitleUseCase>();
            _mockGetEventsByDateUseCase = new Mock<IGetEventsByDateUseCase>();
            _controller = new EventController(
                _mockGetAllEventsUseCase.Object,
                _mockCreateEventUseCase.Object,
                _mockUpdateEventUseCase.Object,
                _mockDeleteEventUseCase.Object,
                _mockGetEventByIdUseCase.Object,
                _mockGetEventByTitleUseCase.Object,
                _mockGetEventsByDateUseCase.Object
              );
        }

        [Fact]
        public void Get_ShouldReturnOkResult_WhenEventsExist()
        {
            // Arrange
            var eventDtos = new List<EventDto>
            {
                new EventDto { Id = 1, Title = "Event 1" },
                new EventDto { Id = 2, Title = "Event 2" }
            };
            _mockGetAllEventsUseCase.Setup(u => u.Execute()).Returns(eventDtos);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<IEnumerable<EventDto>>>(okResult.Value);
            Assert.Equal(2, returnValue.Data.ToList().Count);
            Assert.Equal("Events found successfully", returnValue.Message);
            Assert.Equal(200, returnValue.Status);


        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Put(1, new UpdateEventDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<Dictionary<string, string[]>>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Validation failed. Please check the provided data.", response.Message);
            Assert.Single(response.Data);
        }

        [Fact]
        public void Put_ReturnsOk_WhenEventIsUpdatedSuccessfully()
        {
            // Arrange
            var updatedEventDto = new UpdateEventDto
            {
                Title = "Updated Event",
                Description = "Updated Description",
                Date = new DateTime(2023, 7, 1),
                Time = new TimeSpan(14, 0, 0),
                Location = "Updated Location"
            };
            var updatedEvent = new Event(
                updatedEventDto.Title,
                updatedEventDto.Description,
                updatedEventDto.Date,
                updatedEventDto.Time,
                updatedEventDto.Location
            )
            {
                Id = 1
            };
            _mockUpdateEventUseCase.Setup(x => x.Execute(updatedEventDto)).Returns(updatedEvent);

            // Act
            var result = _controller.Put(1, updatedEventDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<Event>>(okResult.Value);
            Assert.Equal(200, response.Status);
            Assert.Equal("Event updated successfully.", response.Message);
            Assert.Equal(updatedEvent, response.Data);
        }


        [Fact]
        public void Put_ReturnsNotFound_WhenEventIsNotFound()
        {
            // Arrange
            var updatedEventDto = new UpdateEventDto { Title = "Updated Event" };
            _mockUpdateEventUseCase.Setup(x => x.Execute(updatedEventDto)).Throws(new KeyNotFoundException("Event not found"));

            // Act
            var result = _controller.Put(1, updatedEventDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.Equal("Event not found", response.Message);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            var updatedEventDto = new UpdateEventDto { Title = "Updated Event" };
            _mockUpdateEventUseCase.Setup(x => x.Execute(updatedEventDto)).Throws(new ArgumentException("Invalid argument"));

            // Act
            var result = _controller.Put(1, updatedEventDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Invalid argument", response.Message);
        }

        [Fact]
        public void Put_ReturnsConflict_WhenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            var updatedEventDto = new UpdateEventDto { Title = "Updated Event" };
            _mockUpdateEventUseCase.Setup(x => x.Execute(updatedEventDto)).Throws(new InvalidOperationException("Operation invalid"));

            // Act
            var result = _controller.Put(1, updatedEventDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<Response<string>>(conflictResult.Value);
            Assert.Equal(409, response.Status);
            Assert.Equal("Operation invalid", response.Message);
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
            var response = Assert.IsType<Response<Event>>(createdAtActionResult.Value);
            Assert.Equal(201, response.Status);
            Assert.Equal("Event created successfully.", response.Message);
            Assert.Null(response.Errors);
            Assert.Equal(createdEvent.Id, response.Data.Id);
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
            var response = Assert.IsType<Response<Dictionary<string, string[]>>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Validation failed. Please check the provided data.", response.Message);
            Assert.True(response.Data.ContainsKey("Title"));
            Assert.Contains("Required", response.Data["Title"]);
        }


        [Fact]
        public async Task Delete_ShouldReturnOk_WhenEventIsDeleted()
        {
            // Arrange
            var eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.Execute(eventId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<string>>(okResult.Value);
            Assert.Equal(200, response.Status);
            Assert.Equal("Event successfully removed", response.Message);
            Assert.Null(response.Errors);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenEventWasNotDeleted()
        {
            // Arrange
            var eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.Execute(eventId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Event was not removed", response.Message);
            Assert.Null(response.Errors);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            _mockDeleteEventUseCase.Setup(x => x.Execute(eventId)).Throws(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.NotNull(response.Message); 
            Assert.Null(response.Errors);
            Assert.Null(response.Data);
        }


        [Fact]
        public void GetEventById_ShouldReturnOkResult_WhenEventExists()
        {
            // Arrange
            int eventId = 1;
            var eventDto = new EventDto
            {
                Id = eventId,
                Title = "Sample Event",
                Description = "This is a test event",
                Date = DateTime.Today,
                Time = TimeSpan.FromHours(10),
                Location = "Sample Location"
            };
            _mockGetEventByIdUseCase.Setup(u => u.Execute(eventId)).Returns(eventDto);

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<EventDto>>(okResult.Value);
            Assert.Equal(eventDto, returnValue.Data);
        }

    }
}