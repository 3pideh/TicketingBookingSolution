using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Domain;
using TicketingSolution.Core.Enum;
using TicketingSolution.Core.Handler;
using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Test
{
    public class Ticket_Booking_Request_Handler_Test
    {
        private readonly TicketBookingRequestHandler _handler;
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingService> _ticketBookingServiceMock;
        private List<Ticket> _avialbleTickets;

        public Ticket_Booking_Request_Handler_Test()
        {
            //Arrange


            _request = new TicketBookingRequest()
            {

                Name = "Test",
                Family = "TestFamily",
                Email = "Test@gmail.com",
                Date = DateTime.Now
            };

            _avialbleTickets = new List<Ticket>() { new Ticket() { Id = 1 } };

            _ticketBookingServiceMock = new Mock<ITicketBookingService>();
            _ticketBookingServiceMock.Setup(x => x.GetAvailableTicket(_request.Date))
                .Returns(_avialbleTickets);

            _handler = new TicketBookingRequestHandler(_ticketBookingServiceMock.Object);

        }

        [Fact]
        public void Should_Return_Ticket_Booking_Response_With_Request_Values()
        {

            //Act
            ServiceBookingResult result = _handler.BookService(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, _request.Name);
            Assert.Equal(result.Family, _request.Family);
            Assert.Equal(result.Email, _request.Email);

            //Assert By Shouldly
            result.ShouldNotBeNull();
            result.Email.ShouldBe(_request.Email);
            result.Name.ShouldBe(_request.Name);
            result.Family.ShouldBe(_request.Family);



        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception =
                Should.Throw<ArgumentNullException>(() => _handler.BookService(null));

            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_Ticket_Booking_Request()
        {
            //Act
            TicketBooking Savedbooking = null;
            _ticketBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
                .Callback<TicketBooking>
                (booking =>
                {
                    Savedbooking = booking;
                });

            _handler.BookService(_request);

            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);

            //Assert By Shouldly
            Savedbooking.ShouldNotBeNull();
            Savedbooking.Email.ShouldBe(_request.Email);
            Savedbooking.Name.ShouldBe(_request.Name);
            Savedbooking.Family.ShouldBe(_request.Family);
            //first empty seat
            Savedbooking.TicketID.ShouldBe(_avialbleTickets.First().Id);



        }

        [Fact]
        public void Should_Not_Save_Ticket_Booking_Request_If_Not_Available()
        {
            _avialbleTickets.Clear();
            //pass request to handle
            _handler.BookService(_request);
            //Assert
            //when it is not available , save never
            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Never);



        }

        [Theory]
        [InlineData(BookingResultFlag.Failure,false)]
        [InlineData(BookingResultFlag.Success,true)]
        //if isavail = true , return success
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _avialbleTickets.Clear();
            }

            var result = _handler.BookService(_request);

            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(null,false)]
        [InlineData(1,true)]
        //if seat is available , save and return id of ticket
        public void Should_Return_TicketBookingId_In_Result(int? ticketBookingId , bool isAvailable)
        {
            if (!isAvailable)
            {
                _avialbleTickets.Clear();
            }
            else
            {
                _ticketBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
               .Callback<TicketBooking>
               (booking =>
               {
                   booking.TicketID = ticketBookingId.Value;
               });

            }


            var result = _handler.BookService(_request);

            result.TicketBookingId.ShouldBe(ticketBookingId);
        }


    }
}
