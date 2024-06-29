using AutoMapper;
using EventMS.Application.DTOs.UsersDto;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.UseCases.UserUseCases
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserUseCase(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public User Execute(ClaimsPrincipal userClaims)
        {
            var userDto = _mapper.Map<UserDto>(userClaims);


            if (_userRepository.GetUserByIdAsync(userDto.Id) != null)
            {
                return _mapper.Map<User>(userDto);
            }

            var newUser = _mapper.Map<User>(userDto);

            _userRepository.AddUserAsync(newUser);
            return newUser;
        }


    }
}
