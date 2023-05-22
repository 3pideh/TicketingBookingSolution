using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Handler
{
    public interface ITicketBookingRequestHandler
    {
        ITicketBookingService TicketBookingService { get; }

        ServiceBookingResult BookService(TicketBookingRequest bookingRequest);
    }
}