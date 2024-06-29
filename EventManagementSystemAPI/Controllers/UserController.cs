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

        [HttpPost("events/tickets-type{ticketTypeId}")]
        public async Task<IActionResult> PurchaseATicket(int ticketTypeId)
        {

            try
            {

                var user = _createUserUseCase.Execute(User);

                var ticket = _purchaseTicketUseCase.Execute(ticketTypeId, user.Id);

                return CreatedAtAction(nameof(PurchaseATicket), new { id = ticket.Id }, new Response<Ticket>(
                    201,
                    "Ticket purchased succesfully.",
                    ticket
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string>(
                    404,
                    "There are not tickets availbale",
                    null
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new Response<string>(
                    400,
                    ex.Message,
                    null
                ));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new Response<string>(
                    409,
                    ex.Message,
                    null
                ));
            }

        }
    }
}
