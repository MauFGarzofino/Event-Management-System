//using EventMS.Domain.Entities;
//using EventMS.Domain.Interfaces;
//using EventMS.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EventMS.Infrastructure.Repositories
//{
//    public class TicketRepository : ITicketRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public TicketRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IEnumerable<Ticket> GetAllTickets()
//        {
//            return _context.Tickets.Include(t => t.Event).Include(t => t.User).ToList();
//        }

//        public void AddTicket(Ticket newTicket)
//        {
//            newTicket.PurchaseDate = DateTime.Now;
//            _context.Tickets.Add(newTicket);
//            _context.SaveChanges();
//        }

//        //public void UpdateTicket(Ticket updatedTicket)
//        //{
//        //    var local = _context.Set<Ticket>()
//        //        .Local
//        //        .FirstOrDefault(entry => entry.Id.Equals(updatedTicket.Id));

//        //    if (local != null)
//        //    {
//        //        _context.Entry(local).State = EntityState.Detached;
//        //    }
//        //    _context.Entry(updatedTicket).State = EntityState.Modified;
//        //    _context.SaveChanges();
//        //}

//        public Ticket GetTicketById(int id)
//        {
//            return _context.Tickets.Include(t => t.Event).Include(t => t.User).FirstOrDefault(t => t.Id == id);
//        }

//        public bool TicketExists(int id)
//        {
//            return _context.Tickets.Any(t => t.Id == id);
//        }
//    }
//}
