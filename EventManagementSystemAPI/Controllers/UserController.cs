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

        public UserController(IGetAllUsersUseCase getAllUsersUseCase)
        {
            _getAllUsersUseCase = getAllUsersUseCase;
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
    }
}
