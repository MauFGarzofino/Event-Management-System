﻿using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.DTOs.UsersDto;
using EventMS.Application.Ports;
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
    ppublic class GetEventByIdDetailsTests
    {
        [Fact]
        public void Execute_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event("Sample Event", "This is a test event", DateTime.Today, TimeSpan.FromHours(10), "Sample Location")
            {
                Id = eventId
            };

            var eventDto = new EventDto
            {
                Id = eventId,
                Title = "Sample Event",
                Description = "This is a test event",
                Date = DateTime.Today,
                Time = TimeSpan.FromHours(10),
                Location = "Sample Location"
            };

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(repo => repo.GetEventDetailsById(eventId))
                               .Returns(eventEntity);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<EventDto>(It.IsAny<Event>())).Returns(eventDto);

            var useCase = new GetEventByIdUseCase(mockEventRepository.Object, mockMapper.Object);

            // Act
            var result = useCase.Execute(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventDto.Id, result.Id);
            Assert.Equal(eventDto.Title, result.Title);
            Assert.Equal(eventDto.Description, result.Description);
            Assert.Equal(eventDto.Date, result.Date);
            Assert.Equal(eventDto.Time, result.Time);
            Assert.Equal(eventDto.Location, result.Location);
        }

        [Fact]
        public void Execute_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository.Setup(repo => repo.GetEventDetailsById(eventId))
                               .Returns((Event)null);

            var mockMapper = new Mock<IMapper>();

            var useCase = new GetEventByIdUseCase(mockEventRepository.Object, mockMapper.Object);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => useCase.Execute(eventId));
            Assert.Equal($"Event with id '{eventId}' not found.", exception.Message);
        }
    }
}
