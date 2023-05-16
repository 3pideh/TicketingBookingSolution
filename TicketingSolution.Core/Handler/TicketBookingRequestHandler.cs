using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;
using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Handler
{
    public class TicketBookingRequestHandler
    {
        private readonly ITicketBookingService _ticketBookingService;
        public TicketBookingRequestHandler(ITicketBookingService ticketBookingService)
        {
            this._ticketBookingService = ticketBookingService;
        }

        public ITicketBookingService TicketBookingService { get; }

        public ServiceBookingResult BookService(TicketBookingRequest bookingRequest)
        {
            if (bookingRequest is null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableTickets = _ticketBookingService.GetAvailableTicket(bookingRequest.Date);

            if (availableTickets.Any())
            {
                _ticketBookingService.Save(CreateTicketBookingObject<TicketBooking>(bookingRequest));
            }

            
            //_ticketBookingService.Save(new TicketBooking()
            //{
            //    Name = bookingRequest.Name,
            //    Family = bookingRequest.Family,
            //    Email = bookingRequest.Email
            //});

            return CreateTicketBookingObject<ServiceBookingResult>(bookingRequest);

            //return new ServiceBookingResult
            //{
            //    Name = bookingRequest.Name,
            //    Family = bookingRequest.Family,
            //    Email = bookingRequest.Email
            //};
        }


        private static TTicketBooking CreateTicketBookingObject<TTicketBooking>(TicketBookingRequest bookingRequest) where TTicketBooking
            : ServiceBookingBase, new()
        {
            return new TTicketBooking
            {
                Name = bookingRequest.Name,
                Family = bookingRequest.Family,
                Email = bookingRequest.Email
            };
        }
    }
}