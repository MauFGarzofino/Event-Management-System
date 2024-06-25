    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Role { get; set; }
    }
}
