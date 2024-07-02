using EventManagementSystemAPI.Controllers;
using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.Ports;
using EventMS.Application.Ports.Ticket;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
        private readonly Mock<IGetUserTicketsUseCase> _mockGetUserTicketsUseCase;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockPurchaseTicketUseCase = new Mock<IPurchaseTicketUseCase>();
            _mockCreateUserUseCase = new Mock<ICreateUserUseCase>();
            _mockGetUserTicketsUseCase = new Mock<IGetUserTicketsUseCase>();
            _controller = new TicketController(_mockPurchaseTicketUseCase.Object, _mockCreateUserUseCase.Object, _mockGetUserTicketsUseCase.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{new Claim(ClaimTypes.NameIdentifier, "1")}, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
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

        [Fact]
        public async Task GetUserTickets_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.GetUserTickets();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<Response<string>>(unauthorizedResult.Value);
            Assert.Equal(401, response.Status);
            Assert.Equal("User not authenticated.", response.Message);
        }

        [Fact]
        public async Task GetUserTickets_ReturnsOk_WhenNoTicketsFound()
        {
            // Arrange
            var userId = "1";
            _mockGetUserTicketsUseCase.Setup(x => x.Execute(userId)).ReturnsAsync(new List<TicketDto>());

            // Act
            var result = await _controller.GetUserTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<IEnumerable<TicketDto>>>(okResult.Value);
            Assert.Equal(200, response.Status);
            Assert.Equal("No tickets found for the user.", response.Message);
            Assert.Empty(response.Data);
        }

    }
}
