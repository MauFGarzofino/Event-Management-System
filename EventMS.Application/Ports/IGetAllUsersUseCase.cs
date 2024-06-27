using EventMS.Application.DTOs;
using EventMS.Application.DTOs.UsersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface IGetAllUsersUseCase
    {
        IEnumerable<UserDto> Execute();
    }
}
