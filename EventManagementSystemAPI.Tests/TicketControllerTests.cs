using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemAPI.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<IPurchaseTicketUseCase> _mockPurchaseTicketUseCase;
        private readonly Mock<ICreateUserUseCase> _mockCreateUserUseCase;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockPurchaseTicketUseCase = new Mock<IPurchaseTicketUseCase>();
            _mockCreateUserUseCase = new Mock<ICreateUserUseCase>();
            _controller = new TicketController(_mockPurchaseTicketUseCase.Object, _mockCreateUserUseCase.Object);
        }

        [Fact]
        public async Task PurchaseATicket_ReturnsCreatedResult()
        {
            // Arrange
            int ticketTypeId = 1;
            int eventId = 2;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }, "mock"));

            var expectedTicket = new Ticket { Id = 1, EventId = 2 };
            _mockPurchaseTicketUseCase.Setup(x => x.Execute(ticketTypeId, user, eventId)).ReturnsAsync(expectedTicket);

            // Act
            var result = await _controller.PurchaseATicket(ticketTypeId, eventId);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<Response<Ticket>>(createdAtActionResult.Value);
            Assert.Equal(201, response.Status);
            Assert.Equal("Ticket purchased succesfully.", response.Message);
            Assert.Equal(expectedTicket, response.Data);
        }

        [Fact]
        public async Task PurchaseATicket_InsufficientTickets_ReturnsBadRequest()
        {
            // Arrange
            int ticketTypeId = 1;
            int eventId = 2;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            }, "mock"));

            _mockPurchaseTicketUseCase.Setup(x => x.Execute(ticketTypeId, user, eventId)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.PurchaseATicket(ticketTypeId, eventId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<string>>(badRequestResult.Value);
            Assert.Equal(400, response.Status);
            Assert.Equal("Insufficient tickets available.", response.Message);
            Assert.Null(response.Data);
        }

        // Additional test cases for exception handling can be added similarly
    }
}
