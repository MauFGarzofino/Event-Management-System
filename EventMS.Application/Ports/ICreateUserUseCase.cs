using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface ICreateUserUseCase
    {
        Task<User> Execute(ClaimsPrincipal userClaims);
    }
}
