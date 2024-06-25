using EventManagementSystemAPI.Controllers;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemAPI.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IGetAllUsersUseCase> _mockGetAllUsersUseCase;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockGetAllUsersUseCase = new Mock<IGetAllUsersUseCase>();
            _controller = new UserController(_mockGetAllUsersUseCase.Object);
        }

        [Fact]
        public void Get_ShouldReturnOkResult_WhenUsersExist()
        {
            // Arrange
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = "1", Name = "John", Surname = "Doe", Email = "john@example.com", Nickname = "johnd", Role = "User" },
                new UserDto { Id = "2", Name = "Jane", Surname = "Doe", Email = "jane@example.com", Nickname = "janed", Role = "Admin" }
            };
            _mockGetAllUsersUseCase.Setup(u => u.Execute()).Returns(userDtos);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void Get_ShouldReturnNoContent_WhenNoUsersExist()
        {
            // Arrange
            var userDtos = new List<UserDto>();
            _mockGetAllUsersUseCase.Setup(u => u.Execute()).Returns(userDtos);

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
