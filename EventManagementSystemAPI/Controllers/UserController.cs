using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
using EventMS.Application.DTOs.UsersDto;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;
        private readonly ICreateUserUseCase _createUserUseCase;

        public UserController(IGetAllUsersUseCase getAllUsersUseCase, IGetUserByIdUseCase getUserByIdUseCase)
        {
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _getAllUsersUseCase.Execute();

            if (!users.Any())
            {
                return NoContent();
            }

            return Ok(new Response<IEnumerable<UserDto>>(200, "Users found successfully",users));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _getUserByIdUseCase.ExecuteAsync(userId);
            if (user == null)
            {
                return NotFound(new Response<string>(
                    404,
                    "User not found",
                    null
                ));
            }

            return Ok(new Response<User>(200, "User found successfully", user));
        }
    }
}
