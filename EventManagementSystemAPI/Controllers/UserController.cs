using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;

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

            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _getUserByIdUseCase.ExecuteAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var response = new
            {
                userId = user.Id,
                username = user.Nickname,
                email = user.Email,
                registeredAt = user.RegisteredAt
            };
            return Ok(response);

        }
    }
}
