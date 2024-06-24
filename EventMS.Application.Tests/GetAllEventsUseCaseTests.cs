﻿using AutoMapper;
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
using Xunit;

namespace EventMS.Application.Tests
{
    public class GetAllEventsUseCaseTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllEventsUseCase _useCase;

        public GetAllEventsUseCaseTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _mockMapper = new Mock<IMapper>();

            _useCase = new GetAllEventsUseCase(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void Execute_ShouldReturnMappedEvents_WhenEventsExist()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event("Event 1", "Description 1", DateTime.Now, TimeSpan.FromHours(1), "Location 1"),
                new Event("Event 2", "Description 2", DateTime.Now, TimeSpan.FromHours(2), "Location 2")
            };
            var eventDtos = new List<EventDto>
            {
                new EventDto { Title = "Event 1", Description = "Description 1", Date = DateTime.Now, Time = TimeSpan.FromHours(1), Location = "Location 1" },
                new EventDto { Title = "Event 2", Description = "Description 2", Date = DateTime.Now, Time = TimeSpan.FromHours(2), Location = "Location 2" }
            };

            _mockRepository.Setup(r => r.GetAllEvents()).Returns(events);
            _mockMapper.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(eventDtos);

            // Act
            var result = _useCase.Execute();

            // Assert
            Assert.Equal(eventDtos, result);
            _mockRepository.Verify(r => r.GetAllEvents(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<EventDto>>(events), Times.Once);
        }

        [Fact]
        public void Execute_ShouldReturnEmptyList_WhenNoEventsExist()
        {
            // Arrange
            var events = new List<Event>();
            var eventDtos = new List<EventDto>();

            _mockRepository.Setup(r => r.GetAllEvents()).Returns(events);
            _mockMapper.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(eventDtos);

            // Act
            var result = _useCase.Execute();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAllEvents(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<EventDto>>(events), Times.Once);
        }
    }
}
