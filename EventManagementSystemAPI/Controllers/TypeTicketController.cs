using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeTicketController : ControllerBase
    {
        private readonly ITypeTicketRepository _typeTicketRepository;
        private readonly IMapper _mapper;

        public TypeTicketController(ITypeTicketRepository typeTicketRepository, IMapper mapper)
        {
            _typeTicketRepository = typeTicketRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<TypeTicketDto> Create(TypeTicketDto typeTicketDto)
        {
            var typeTicket = _mapper.Map<TypeTicket>(typeTicketDto);
            _typeTicketRepository.AddTypeTicket(typeTicket);
            var createdTypeTicketDto = _mapper.Map<TypeTicketDto>(typeTicket);
            return CreatedAtAction(nameof(Create), new { id = typeTicket.Id }, createdTypeTicketDto);
        }

    }
}
