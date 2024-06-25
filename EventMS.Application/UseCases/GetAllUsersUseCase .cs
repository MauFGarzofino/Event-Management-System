using AutoMapper;
using EventMS.Application.DTOs;
using EventMS.Application.Ports;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersUseCase(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserDto> Execute()
        {
            var users = _userRepository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
