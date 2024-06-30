using EventManagementSystemAPI.Models;
using EventMS.Application.DTOs;
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
        private readonly IPurchaseTicketUseCase _purchaseTicketUseCase;

        public UserController(IGetAllUsersUseCase getAllUsersUseCase, IGetUserByIdUseCase getUserByIdUseCase, ICreateUserUseCase createUserUseCase, IPurchaseTicketUseCase purchaseTicketUseCase)
        {
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserByIdUseCase = getUserByIdUseCase;
            _createUserUseCase = createUserUseCase;
            _purchaseTicketUseCase = purchaseTicketUseCase;
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
            };
            return Ok(response);
        }
    }
}
