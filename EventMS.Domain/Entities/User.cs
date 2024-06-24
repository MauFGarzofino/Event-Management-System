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
        private List<Ticket> _tickets;

        // Constructor sin parámetros requerido por EF
        private User()
        {
            _tickets = new List<Ticket>();
        }

        public User(string userId, string name, string surname, string email, string nickname, string role)
        {
            Id = userId;
            _name = name;
            _surname = surname;
            _email = email;
            _nickname = nickname;
            _role = role;
            _tickets = new List<Ticket>();
        }

        public string Id { get; private set; } // This is the 'sub' from the token
        public string Name => _name;
        public string Surname => _surname;
        public string Email => _email;
        public string Nickname => _nickname;
        public string Role => _role;
        public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

        public void UpdateContactInformation(string name, string surname, string email, string nickname, string role)
        {
            _name = name;
            _surname = surname;
            _email = email;
            _nickname = nickname;
            _role = role;
        }

        public void AddTicket(Ticket ticket)
        {
            _tickets.Add(ticket);
        }
    }
}