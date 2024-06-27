using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.DTOs.UsersDto;
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
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllUsersUseCase _useCase;

        public GetAllUsersUseCaseTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _useCase = new GetAllUsersUseCase(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void Execute_ShouldReturnMappedUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User("1", "John", "Doe", "john@example.com", "johnd", "User"),
                new User("2", "Jane", "Doe", "jane@example.com", "janed", "Admin")
            };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = "1", Name = "John", Surname = "Doe", Email = "john@example.com", Nickname = "johnd", Role = "User" },
                new UserDto { Id = "2", Name = "Jane", Surname = "Doe", Email = "jane@example.com", Nickname = "janed", Role = "Admin" }
            };

            _mockRepository.Setup(r => r.GetAllUsers()).Returns(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = _useCase.Execute();

            // Assert
            Assert.Equal(userDtos, result);
            _mockRepository.Verify(r => r.GetAllUsers(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<UserDto>>(users), Times.Once);
        }

        [Fact]
        public void Execute_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<User>();
            var userDtos = new List<UserDto>();

            _mockRepository.Setup(r => r.GetAllUsers()).Returns(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = _useCase.Execute();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAllUsers(), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<UserDto>>(users), Times.Once);
        }
    }
}
