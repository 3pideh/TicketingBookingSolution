using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Domain;

namespace TicketingSolution.Persistence.Repositories
{
    public class TicketBookingService : ITicketBookingService
    {
        public TicketingSolutionDbContext _context { get; }
        public TicketBookingService(TicketingSolutionDbContext dbContext) {

            this._context = dbContext; 
        }

        

        public IEnumerable<Ticket> GetAvailableTicket(DateTime dateTime)
        {
            var unavailableTickets = _context.TicketBookings
                .Where(t => t.Date  == dateTime)
                .Select(t => t.TicketID)
                .ToList();

            var availableTickets = _context.Tickets
                .Where(q => !unavailableTickets.Contains(q.Id) == false)
                .ToList();

            _context.Tickets.
                Where(q => !q.TicketBooking.Any(x => x.Date == dateTime)).ToList();

            return availableTickets;
        }

        public void Save(TicketBooking ticketBooking)
        {
            _context.TicketBookings.Add(ticketBooking);
            _context.SaveChanges();
        }
    }
}
