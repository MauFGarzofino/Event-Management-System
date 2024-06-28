﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Domain.Entities
{
    public class Ticket
    {
        // Propiedades
        public int Id { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public TicketStatus Status { get; private set; }
        public int EventId { get; private set; }
        public Event Event { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public int TypeTicketId { get; private set; }
        public TypeTicket TypeTicket { get; private set; }

        // Constructor sin parámetros requerido por EF
        private Ticket() { }

        // Constructor principal
        public Ticket(int typeTicketId, Event ev, User user)
        {
            
            PurchaseDate = DateTime.Now;
            Status = TicketStatus.Purchased;
            Event = ev;
            User = user;
            EventId = ev.Id;
            UserId = user.Id;
            TypeTicketId = typeTicketId;
        }

        public void MarkAsCheckedIn()
        {
            if (Status != TicketStatus.Purchased)
            {
                throw new InvalidOperationException("Only purchased tickets can be checked in.");
            }
            Status = TicketStatus.CheckedIn;
        }
    }

    public enum TicketStatus
    {
        Purchased,
        CheckedIn,
        Cancelled
    }
}