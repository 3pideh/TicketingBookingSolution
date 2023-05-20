

using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Model;
using TicketingSolution.Domain.BaseModels;
using TicketingSolution.Domain;

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
            var result = CreateTicketBookingObject<ServiceBookingResult>(bookingRequest);
            if (availableTickets.Any())
            {
                var Ticket = availableTickets.First();
                var TicketBooking = CreateTicketBookingObject<TicketBooking>(bookingRequest);
                TicketBooking.TicketID = Ticket.Id;
                _ticketBookingService.Save(TicketBooking);
                result.TicketBookingId = TicketBooking.TicketID;
                result.Flag = Enum.BookingResultFlag.Success;
            }

            else result.Flag = Enum.BookingResultFlag.Failure;


            //_ticketBookingService.Save(new TicketBooking()
            //{
            //    Name = bookingRequest.Name,
            //    Family = bookingRequest.Family,
            //    Email = bookingRequest.Email
            //});

            return result;

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