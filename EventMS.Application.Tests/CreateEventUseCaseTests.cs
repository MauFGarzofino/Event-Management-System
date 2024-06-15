using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.UseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Moq;

namespace EventMS.Application.Tests
{
    public class CreateEventUseCaseTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly IMapper _mapper;
        private readonly CreateEventUseCase _createEventUseCase;

        public CreateEventUseCaseTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<EventDto, Event>();
            });
            _mapper = mapperConfig.CreateMapper();

            _createEventUseCase = new CreateEventUseCase(_mockEventRepository.Object, _mapper);
        }

        [Fact]
        public void Execute_ShouldThrowArgumentException_WhenMandatoryFieldsAreMissing()
        {
            var newEventDto = new EventDto
            {
                Title = "",
                Date = DateTime.MinValue,
                Time = TimeSpan.Zero,
                Location = ""
            };

            Assert.Throws<ArgumentException>(() => _createEventUseCase.Execute(newEventDto));
        }

        [Fact]
        public void Execute_ShouldThrowInvalidOperationException_WhenEventAlreadyExists()
        {
            var newEventDto = new EventDto
            {
                Title = "Existing Event",
                Date = new DateTime(2024, 8, 15),
                Time = new TimeSpan(11, 0, 0),
                Location = "City Library"
            };

            _mockEventRepository.Setup(repo => repo.EventExists(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(true);

            Assert.Throws<InvalidOperationException>(() => _createEventUseCase.Execute(newEventDto));
        }

        [Fact]
        public void Execute_ShouldAddEvent_WhenEventIsValid()
        {
            var newEventDto = new EventDto
            {
                Title = "",
                Date = new DateTime(2024, 8, 15),
                Time = new TimeSpan(11, 0, 0),
                Location = "City Library"
            };

            _mockEventRepository.Setup(repo => repo.EventExists(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(false);

            var createdEvent = _createEventUseCase.Execute(newEventDto);

            _mockEventRepository.Verify(repo => repo.AddEvent(It.IsAny<Event>()), Times.Once);
            Assert.NotNull(createdEvent);
            Assert.Equal(newEventDto.Title, createdEvent.Title);
        }
    }
}