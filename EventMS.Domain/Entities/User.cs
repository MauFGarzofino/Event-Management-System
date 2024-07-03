using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class User
    {
        private string _name;
        private string _surname;
        private string _email;
        private string _nickname;
        private string _role;

        private User()
        {
            Tickets = new List<Ticket>();
            EventRegistrations = new List<EventRegistration>();
        }

        public User(string userId, string name, string surname, string email, string nickname, string role)
        {
            Id = userId;
            _name = name;
            _surname = surname;
            _email = email;
            _nickname = nickname;
            _role = role;
            Tickets = new List<Ticket>();
            EventRegistrations = new List<EventRegistration>();
        }

        public string Id { get; private set; } // This is the 'sub' from the token
        public string Name => _name;
        public string Surname => _surname;
        public string Email => _email;
        public string Nickname => _nickname;
        public string Role => _role;
        public ICollection<Ticket> Tickets { get; private set; }
        public ICollection<EventRegistration> EventRegistrations { get; private set; }
    }
}