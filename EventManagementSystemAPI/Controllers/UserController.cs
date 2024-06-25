using EventMS.Application.DTOs.UsersDto;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateUserUseCase _userUseCase;

        public UserController(IUserRepository userRepository, ICreateUserUseCase userUseCase)
        {
            _userRepository = userRepository;
            _userUseCase = userUseCase;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser()
        {
            var user = _userUseCase.Execute(User);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

        }

    }
}
