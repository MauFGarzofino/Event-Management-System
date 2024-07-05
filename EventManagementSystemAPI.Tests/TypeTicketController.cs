using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task Post_ReturnsCreated_WhenTypeTicketCreatedSuccessfully()
        {
            // Arrange
            int eventId = 1;
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50};
            var createdTypeTicket = new TypeTicket { Id = 1, Name = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50, EventId = 1 };
            _mockCreateTypeTicketUseCase.Setup(x => x.ExecuteAsync(newTypeTicketDto, eventId)).ReturnsAsync(createdTypeTicket);

            // Act
            var result = await _controller.Post(newTypeTicketDto, eventId);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<Response<TypeTicket>>(createdResult.Value);
            Assert.Equal(201, response.Status);
            Assert.Equal("Type ticket created successfully.", response.Message);
            Assert.Equal(createdTypeTicket, response.Data);
        }

        [Fact]
        public async Task Post_ReturnsNotFound_WhenKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            int eventId = 1;
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50 };
            _mockCreateTypeTicketUseCase.Setup(x => x.ExecuteAsync(newTypeTicketDto, eventId)).ThrowsAsync(new KeyNotFoundException("Event not found"));

            // Act
            var result = await _controller.Post(newTypeTicketDto, eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.Equal("Event not found", response.Message);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
        {
            // Arrange
            int eventId = 1;
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50 };
            _mockCreateTypeTicketUseCase.Setup(x => x.ExecuteAsync(newTypeTicketDto, eventId)).ThrowsAsync(new ArgumentException("Invalid argument"));

            // Act
            var result = await _controller.Post(newTypeTicketDto, eventId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Invalid argument", response.Message);
        }

        [Fact]
        public async Task Post_ReturnsConflict_WhenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            int eventId = 1;
            var newTypeTicketDto = new TypeTicketDto { Name = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50 };
            _mockCreateTypeTicketUseCase.Setup(x => x.ExecuteAsync(newTypeTicketDto, eventId)).ThrowsAsync(new InvalidOperationException("Operation invalid"));

            // Act
            var result = await _controller.Post(newTypeTicketDto, eventId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<Response<string>>(conflictResult.Value);
            Assert.Equal(409, response.Status);
            Assert.Equal("Operation invalid", response.Message);
        }

        [Fact]
        public async Task GetTicketTypeCounts_ReturnsNotFound_WhenNoTicketTypesFound()
        {
            // Arrange
            var eventId = 1;
            _mockGetTicketTypeCountsUseCase.Setup(x => x.ExecuteAsync(eventId)).ReturnsAsync(new List<TicketTypeCountDto>());

            // Act
            var result = await _controller.GetTicketTypeCounts(eventId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Response<string>>(notFoundResult.Value);
            Assert.Equal(404, response.Status);
            Assert.Equal("No ticket types found for the specified event.", response.Message);
        }

        [Fact]
        public async Task GetTicketTypeCounts_ReturnsOk_WhenTicketTypeCountsRetrievedSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var ticketTypeCounts = new List<TicketTypeCountDto>
            {
                new TicketTypeCountDto { TypeName = "VIP", Description = "VIP Ticket", Price = 100, QuantityAvailable = 50 },
                new TicketTypeCountDto { TypeName = "General", Description = "General Ticket", Price = 50, QuantityAvailable = 100 }
            };
            _mockGetTicketTypeCountsUseCase.Setup(x => x.ExecuteAsync(eventId)).ReturnsAsync(ticketTypeCounts);

            // Act
            var result = await _controller.GetTicketTypeCounts(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<IEnumerable<TicketTypeCountDto>>>(okResult.Value);
            Assert.Equal(200, response.Status);
            Assert.Equal("Ticket type counts retrieved successfully.", response.Message);
            Assert.Equal(ticketTypeCounts, response.Data);
        }
    }
}
