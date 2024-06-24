using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventMS.Application.Ports;
namespace EventManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGetUserByIdUseCase _getUserByIdUseCase;

        public UserController(IGetUserByIdUseCase getUserByIdUseCase)
        {
            _getUserByIdUseCase = getUserByIdUseCase;
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
