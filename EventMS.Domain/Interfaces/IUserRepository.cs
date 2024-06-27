using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Interfaces
{
    public interface IUserRepository
    {

        void AddUserAsync(User user);
        IEnumerable<User> GetAllUsers();
        Task<User> GetUserByIdAsync(string userId);

    }
}
