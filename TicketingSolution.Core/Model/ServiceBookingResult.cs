using TicketingSolution.Core.Enum;
using TicketingSolution.Domain.BaseModels;

namespace TicketingSolution.Core.Model
{
    public class ServiceBookingResult : ServiceBookingBase
    {
        public int? TicketBookingId { get; set; }
        public BookingResultFlag Flag { get; set; }
    }
}