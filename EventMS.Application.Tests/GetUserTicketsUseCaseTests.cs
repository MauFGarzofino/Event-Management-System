using AutoMapper;
using EventMS.Application.DTOs.Tickets;
using EventMS.Application.UseCases.Ticket;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Tests
{
    public class GetUserTicketsUseCaseTests
    {
        private readonly Mock<ITicketRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserTicketsUseCase _useCase;

        public GetUserTicketsUseCaseTests()
        {
            _mockRepository = new Mock<ITicketRepository>();
            _mockMapper = new Mock<IMapper>();
            _useCase = new GetUserTicketsUseCase(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnMappedTickets_WhenTicketsExist()
        {
            // Arrange
            var userId = "user1";
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, EventId = 1, UserId = userId, TypeTicketId = 1 },
                new Ticket { Id = 2, EventId = 2, UserId = userId, TypeTicketId = 2 }
            };
            var ticketDtos = new List<TicketDto>
            {
                new TicketDto { Id = 1, EventId = 1, UserId = userId, TypeTicketId = 1 },
                new TicketDto { Id = 2, EventId = 2, UserId = userId, TypeTicketId = 2 }
            };

            _mockRepository.Setup(r => r.GetTicketsByUserId(userId)).ReturnsAsync(tickets);
            _mockMapper.Setup(m => m.Map<IEnumerable<TicketDto>>(tickets)).Returns(ticketDtos);

            // Act
            var result = await _useCase.Execute(userId);

            // Assert
            Assert.Equal(ticketDtos, result);
            _mockRepository.Verify(r => r.GetTicketsByUserId(userId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<TicketDto>>(tickets), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnEmptyList_WhenNoTicketsExist()
        {
            // Arrange
            var userId = "user1";
            var tickets = new List<Ticket>();
            var ticketDtos = new List<TicketDto>();

            _mockRepository.Setup(r => r.GetTicketsByUserId(userId)).ReturnsAsync(tickets);
            _mockMapper.Setup(m => m.Map<IEnumerable<TicketDto>>(tickets)).Returns(ticketDtos);

            // Act
            var result = await _useCase.Execute(userId);

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetTicketsByUserId(userId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<TicketDto>>(tickets), Times.Once);
        }
    }
}
