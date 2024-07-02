using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemAPI.Tests
{
    public class TicketsControllerTests
    {
        private readonly Mock<ICreateTypeTicketUseCase> _mockCreateTypeTicketUseCase;
        private readonly Mock<IGetTicketTypeCountsUseCase> _mockGetTicketTypeCountsUseCase;
        private readonly TypeTicketController _controller;

        public TicketsControllerTests()
        {
            _mockCreateTypeTicketUseCase = new Mock<ICreateTypeTicketUseCase>();
            _mockGetTicketTypeCountsUseCase = new Mock<IGetTicketTypeCountsUseCase>();
            _controller = new TypeTicketController(_mockCreateTypeTicketUseCase.Object, _mockGetTicketTypeCountsUseCase.Object);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Post(new TypeTicketDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Post_ReturnsCreated_WhenTypeTicketCreatedSuccessfully()
        {
            // Arrange
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP" };
            var createdTypeTicket = new TypeTicket { Id = 1, Name = "VIP" };
            _mockCreateTypeTicketUseCase.Setup(x => x.Execute(newTypeTicketDto)).Returns(createdTypeTicket);

            // Act
            var result = _controller.Post(newTypeTicketDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<Response<TypeTicket>>(createdResult.Value);
            Assert.Equal(201, response.Status);
            Assert.Equal("Type ticket created successfully.", response.Message);
            Assert.Equal(createdTypeTicket, response.Data);
        }

        [Fact]
        public void Post_ReturnsNotFound_WhenKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP" };
            _mockCreateTypeTicketUseCase.Setup(x => x.Execute(newTypeTicketDto)).Throws(new KeyNotFoundException("Event not found"));

            // Act
            var result = _controller.Post(newTypeTicketDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.Equal("Event not found", response.Message);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP" };
            _mockCreateTypeTicketUseCase.Setup(x => x.Execute(newTypeTicketDto)).Throws(new ArgumentException("Invalid argument"));

            // Act
            var result = _controller.Post(newTypeTicketDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Invalid argument", response.Message);
        }

        [Fact]
        public void Post_ReturnsConflict_WhenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP" };
            _mockCreateTypeTicketUseCase.Setup(x => x.Execute(newTypeTicketDto)).Throws(new InvalidOperationException("Operation invalid"));

            // Act
            var result = _controller.Post(newTypeTicketDto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<Response<string>>(conflictResult.Value);
            Assert.Equal(409, response.Status);
            Assert.Equal("Operation invalid", response.Message);
        }

        [Fact]
        public void GetTicketTypeCounts_ReturnsNotFound_WhenNoTicketTypesFound()
        {
            // Arrange
            var eventId = 1;
            _mockGetTicketTypeCountsUseCase.Setup(x => x.Execute(eventId)).Returns(new List<TicketTypeCountDto>());

            // Act
            var result = _controller.GetTicketTypeCounts(eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.Equal("No ticket types found for the specified event.", response.Message);
        }

        [Fact]
        public void GetTicketTypeCounts_ReturnsOk_WhenTicketTypeCountsRetrievedSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var ticketTypeCounts = new List<TicketTypeCountDto>
            {
                new TicketTypeCountDto { TypeName = "VIP", Count = 10 },
                new TicketTypeCountDto { TypeName = "General", Count = 20 }
            };
            _mockGetTicketTypeCountsUseCase.Setup(x => x.Execute(eventId)).Returns(ticketTypeCounts);

            // Act
            var result = _controller.GetTicketTypeCounts(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<IEnumerable<TicketTypeCountDto>>>(okResult.Value);
            Assert.Equal(200, response.Status);
            Assert.Equal("Ticket type counts retrieved successfully.", response.Message);
            Assert.Equal(ticketTypeCounts, response.Data);
        }
    }
}
