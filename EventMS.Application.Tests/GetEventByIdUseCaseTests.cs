using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.UseCases;
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
    public class GetEventByIdUseCaseTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly GetEventByIdUseCase _getEventByIdUseCase;

        public GetEventByIdUseCaseTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _getEventByIdUseCase = new GetEventByIdUseCase(_mockEventRepository.Object);
        }

        [Fact]
        public void GetEventById_ExistingEvent_ReturnsEventDetailDto()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event
            {
                Id = eventId,
                Title = "Sample Event",
                Description = "Sample Description",
                Date = DateTime.Now.Date,
                Time = DateTime.Now.TimeOfDay,
                Location = "Sample Location"
            };

            _mockEventRepository.Setup(repo => repo.GetEventById(eventId)).Returns(eventEntity);

            // Act
            var result = _getEventByIdUseCase.GetEventById(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.Equal("Sample Event", result.Title);
            Assert.Equal("Sample Description", result.Description);
            Assert.Equal(eventEntity.Date, result.Date);
            Assert.Equal(eventEntity.Time, result.Time);
            Assert.Equal("Sample Location", result.Location);
        }

        [Fact]
        public void GetEventById_NonExistingEvent_ThrowsArgumentException()
        {
            // Arrange
            var eventId = 1;

            _mockEventRepository.Setup(repo => repo.GetEventById(eventId)).Returns((Event)null);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _getEventByIdUseCase.GetEventById(eventId));
        }
    }
}
